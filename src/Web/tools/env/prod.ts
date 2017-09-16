import {EnvConfig} from './env-config.interface';

const ProdConfig: EnvConfig = {
  ENV: 'PROD',
  API: 'http://test-api.minutz.net/api',
  externalLoginPageUrl: 'https://opsoauth.mgsops.net:10251/#/grant?client_id=internal-dev&' +
  'redirect_uri=http%3A%2F%2Flocalhost%3A5555%2F%23%2Ftoken&state=45s9KYgjfj8gD0A2YvfNLw&response_type=code',
  tokenApiEndpointUrl: 'https://opsoauth.mgsops.net:10250/OAuth/Token',
  tokenApiRedirectUrl: 'http://minutz-ui-48a627c6-1.3496b7f3.cont.dockerapp.io/#/token',
  callbackUrl: 'http://test-api.minutz.net/',
  client_id: 'internal-dev',
  client_secret: 'apipassword123'
};

export = ProdConfig;

