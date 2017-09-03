// Feel free to extend this interface
// depending on your app specific config.
export interface EnvConfig {
  API?: string;
  ENV?: string;
  externalLoginPageUrl: string;
  tokenApiEndpointUrl: string;
  tokenApiRedirectUrl: string;
  callbackUrl:string;
  client_id: string;
  client_secret: string;
}

export const Config: EnvConfig = JSON.parse('<%= ENV_CONFIG %>');

