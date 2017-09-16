import {EnvConfig} from './env-config.interface';

const DevConfig: EnvConfig = {
  ENV: 'DEV',
  API: 'http://localhost:65510/api',
  externalLoginPageUrl: 'https://opsoauth.mgsops.net:10251/#/grant?client_id=internal-dev&' +
  'redirect_uri=http%3A%2F%2Flocalhost%3A5555%2F%23%2Ftoken&state=45s9KYgjfj8gD0A2YvfNLw&response_type=code',
  tokenApiEndpointUrl: 'https://opsoauth.mgsops.net:10250/OAuth/Token',
  tokenApiRedirectUrl: 'http://localhost:5555/#/token',
  callbackUrl: 'http://localhost:4200/',
  domain: 'dockerdurban.auth0.com',
  audience: 'https://minutz.net',
  clientID: 'jqdE5upkdDHejU2wJpzHRXYBEwFtsH3N',
  client_secret: 'apipassword123'
};

export = DevConfig;

// clientID: 'jqdE5upkdDHejU2wJpzHRXYBEwFtsH3N',
// domain: 'dockerdurban.auth0.com',
// audience: 'https://minutz.net',
// callbackURL: 'http://localhost:4200/',
// //callbackURL: 'http://test.minutz.net'