import {
  Config
} from '../shared/config/env.config';
import {
  Injectable
} from '@angular/core';
import {
  Router
} from '@angular/router';
declare let auth0: any;
@Injectable()
export class AuthService {
   auth0 : any;  
  public constructor(public router: Router) {
    this.auth0 = new auth0.WebAuth({
      clientID: Config.clientID,
      domain: Config.domain,
      responseType: 'token id_token',
      audience: Config.audience,
      redirectUri: `${Config.callbackUrl}`,
      scope: 'openid profile email' });
    console.log('init auth');
  }
  public login(): void {
    this.auth0.authorize();
  }
  public handleAuthentication(): void {
    this.auth0.parseHash((err:any, authResult: any) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        window.location.hash = '';
        // this.auth0.WebAuth.userinfo(authResult.access_token,function(err: any,user: any){
        //   console.log(user);
        // });
        this.setSession(authResult);
        this.router.navigate(['/home']);
      } else if (err) {
        this.router.navigate(['/home']);
        console.log(err);
        alert(`Error: ${err.error}. Check the console for further details.`);
      }
    });
  }
  public setSession(authResult: any): void {
    // Set the time that the access token will expire at
    const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
    localStorage.setItem('access_token', authResult.accessToken);
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('name', authResult.idTokenPayload.name);
    localStorage.setItem('picture', authResult.idTokenPayload.picture);
    localStorage.setItem('expires_at', expiresAt);
  }
  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');
    // Go back to the home route
    this.router.navigate(['/']);
  }
  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // access token's expiry time
    const expiresAt = JSON.parse(localStorage.getItem('expires_at'));
    return new Date().getTime() < expiresAt;
  }
}
