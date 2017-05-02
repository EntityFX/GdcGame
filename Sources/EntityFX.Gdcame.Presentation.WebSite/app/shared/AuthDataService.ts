namespace Gdcame.Services
{
    export class AuthDataService extends ApiServiceBase implements IAuthenticateService
    {
        private userService: Gdcame.Services.IUserService;
        private apiServiceUri: Services.IApiUriService;
        private localStorage: any;
        private q: angular.IQService;

        public constructor($http: angular.IHttpService, $q: angular.IQService, $localStorage, apiServiceUri: IApiUriService, userService: IUserService) {
            super($http);
            this.apiServiceUri = apiServiceUri;
            this.localStorage = $localStorage;
            this.userService = userService;
            this.q = $q;
        }

        public setCredentials(authData: AuthToken, login): void {
            this.http.defaults.headers.common['Authorization'] = authData.token_type + ' ' + authData.access_token; // jshint ignore:line
            const auth = {
                login: login,
                authData: authData
            };
            this.localStorage.globals = {
                auth: auth,
                apiAddress: this.apiServiceUri.getApiAddressByLogin(login)
            };
        }

        public clearCredentials () {
            delete this.localStorage.globals;
            this.http.defaults.headers.common.Authorization = 'Basic';
        }

        login(authData: Services.AuthData): angular.IHttpPromise<Services.AuthToken> {
            var defer = this.q.defer<Services.AuthToken>();
            this.userService.login(authData)
                .then(result => {
                    this.setCredentials(result.data, authData.login);
                    defer.resolve(result.data);
                }, error => {
                    defer.reject(error.data);
                });
            return defer.promise;
        }

        logout(): angular.IHttpPromise<any> {
            var defer = this.q.defer<Services.AuthToken>();
            this.userService.logout()
                .then(result => {
                    this.clearCredentials();
                    defer.resolve(result.data);
                }, error => {
                    defer.reject(error.data);
                });
            return defer.promise;
        }
    }
}