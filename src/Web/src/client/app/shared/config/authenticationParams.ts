import {Config} from './env.config';

export let AuthenticationParams = {
  roleFilters: Array<any>(),
  externalLoginPage: Config.externalLoginPageUrl,
  tokenApiEndpoint: Config.tokenApiEndpointUrl,
  tokenApiRedirectUrl: Config.tokenApiRedirectUrl,
  client_id: Config.clientID,
  client_secret: Config.client_secret
};
