import { NextResponse } from 'next/server';
import { auth0 } from '@/lib/auth0';

export async function GET() {
  try {
    const session = await auth0.getSession();
    
    if (!session) {
      return NextResponse.json({ user: null }, { status: 401 });
    }

    // Get access token for API calls
    const { token: accessToken } = await auth0.getAccessToken();

    // Fetch user from your .NET backend
    const apiBaseUrl = process.env.API_BASE_URL || 'http://localhost:5050';
    const response = await fetch(`${apiBaseUrl}/api/user`, {
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      // User might not exist in backend yet (first login)
      if (response.status === 404) {
        return NextResponse.json({
          user: {
            auth0Id: session.user.sub,
            email: session.user.email,
            name: session.user.name,
            picture: session.user.picture,
            isNewUser: true, // Frontend can handle first-time setup
          },
        });
      }
      
      console.error('Failed to fetch user from backend:', response.status);
      return NextResponse.json(
        { error: 'Failed to fetch user' },
        { status: response.status }
      );
    }

    const backendUser = await response.json();

    return NextResponse.json({
      user: {
        ...backendUser,
        // Include Auth0 data as well
        picture: session.user.picture,
      },
    });
  } catch (error) {
    console.error('Error in /api/user:', error);
    return NextResponse.json(
      { error: 'Internal server error' },
      { status: 500 }
    );
  }
}
