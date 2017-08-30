import {EnvConfig} from './env-config.interface';

const DevConfig: EnvConfig = {
  ENV: 'DEV',
  API: 'http://localhost/api',
  externalLoginPageUrl: 'https://opsoauth.mgsops.net:10251/#/grant?client_id=internal-dev&' +
  'redirect_uri=http%3A%2F%2Flocalhost%3A5555%2F%23%2Ftoken&state=45s9KYgjfj8gD0A2YvfNLw&response_type=code',
  tokenApiEndpointUrl: 'https://opsoauth.mgsops.net:10250/OAuth/Token',
  tokenApiRedirectUrl: 'http://localhost:5555/#/token',
  client_id: 'internal-dev',
  client_secret: 'apipassword123'
};

export = DevConfig;

