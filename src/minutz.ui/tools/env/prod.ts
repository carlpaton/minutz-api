import {EnvConfig} from './env-config.interface';

const ProdConfig: EnvConfig = {
  ENV: 'PROD',
  API: '#{coreApiUrl}',
  externalLoginPageUrl: '#{externalLoginPageUrl}',
  tokenApiEndpointUrl: '#{tokenApiEndpointUrl}',
  tokenApiRedirectUrl: '#{tokenApiRedirectUrl}',
  client_id: '#{clientId}',
  client_secret: '#{clientSecret}'
};

export = ProdConfig;

