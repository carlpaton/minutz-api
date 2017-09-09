import {Config} from './env.config';

export let AuthenticationParams = {
  roleFilters: Array<any>(),
  externalLoginPage: Config.externalLoginPageUrl,
  tokenApiEndpoint: Config.tokenApiEndpointUrl,
  tokenApiRedirectUrl: Config.tokenApiRedirectUrl,
  client_id: Config.client_id,
  client_secret: Config.client_secret
};
