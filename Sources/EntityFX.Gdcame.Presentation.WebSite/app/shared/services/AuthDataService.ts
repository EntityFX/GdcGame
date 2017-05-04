module Gdcame.Services {
    export class AuthDataService extends ApiServiceBase implements IAuthenticateService, ISessionService {
        private userService: Gdcame.Services.IUserApiService;
        private apiServiceUri: Services.IUriService;
        private localStorage: angular.storage.IStorageService;
        private q: angular.IQService;
        private rootScope: angular.IScope;

        public constructor($rootScope: angular.IScope, $http: angular.IHttpService,
            $q: angular.IQService, $localStorage: angular.storage.IStorageService,
            apiServiceUri: Services.IUriService, userService: Services.IUserApiService) {
            super($http);
            this.apiServiceUri = apiServiceUri;
            this.localStorage = $localStorage;
            this.userService = userService;
            this.q = $q;
            this.rootScope = $rootScope;
        }

        private setCredentials(authData: AuthToken, login: string): void {
            this.http.defaults.headers.common['Authorization'] = authData.token_type + ' ' + authData.access_token; // jshint ignore:line
            const auth = {
                login: login,
                authData: authData
            };
            this.localStorage["globals"] = {
                auth: auth,
                apiAddress: this.apiServiceUri.getApiAddressByLogin(login)
            };
        }

        private clearCredentials() {
            delete this.localStorage["globals"];
            this.http.defaults.headers.common.Authorization = 'Basic';
        }

        login(authData: Services.AuthData): angular.IHttpPromise<Services.AuthToken> {
            var defer = this.q.defer<Services.AuthToken>();
            this.userService.login(authData)
                .then(result => {
                    this.setCredentials(result.data, authData.login);
                    this.rootScope.$broadcast("onLoginSuccess");
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

        getLogin(): string {
            if (this.localStorage["globals"] && this.localStorage["globals"].auth) {
                return this.localStorage["globals"].auth.login;
            }
            return null;
        }
    }

    angular.module("gdCameApp").service("AuthDataService", AuthDataService);

    AuthDataService.$inject = [
        "$rootScope", "$http", "$q", "$localStorage", "UriService", "UserApiService"
    ];
}