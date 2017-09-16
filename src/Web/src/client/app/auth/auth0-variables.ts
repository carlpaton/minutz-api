interface AuthConfig {
    clientID: string;
    domain: string;
    audience: string;
    callbackURL: string;
}
export const AUTH_CONFIG: AuthConfig = {
    clientID: 'jqdE5upkdDHejU2wJpzHRXYBEwFtsH3N',
    domain: 'dockerdurban.auth0.com',
    audience: 'https://minutz.net',
    //callbackURL: 'http://localhost:4200/',
    callbackURL: 'http://test-api.minutz.net'
};
