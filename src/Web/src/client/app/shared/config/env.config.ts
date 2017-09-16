// Feel free to extend this interface
// depending on your app specific config.
export interface EnvConfig {
  API?: string;
  ENV?: string;
  externalLoginPageUrl: string;
  tokenApiEndpointUrl: string;
  tokenApiRedirectUrl: string;
  domain: string;
  audience: string;
  callbackUrl:string;
  clientID: string;
  client_secret: string;
}

export const Config: EnvConfig = JSON.parse('<%= ENV_CONFIG %>');

// clientID: 'jqdE5upkdDHejU2wJpzHRXYBEwFtsH3N',
// domain: 'dockerdurban.auth0.com',
// audience: 'https://minutz.net',
// callbackURL: 'http://localhost:4200/',
// //callbackURL: 'http://test.minutz.net'