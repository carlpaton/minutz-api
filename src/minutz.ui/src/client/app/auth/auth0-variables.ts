interface AuthConfig {
  clientID: string;
  domain: string;
  callbackURL: string;
}
export const AUTH_CONFIG: AuthConfig = {
  clientID: 'spEQucIOEbSBvkixqRHzkKDHNYRaGy3J',
  domain: 'tzatziki-minutz.auth0.com',
  //callbackURL: `${window.location}`
  callbackURL: 'http://dockercloud-81f01559.cloudapp.net'
};
