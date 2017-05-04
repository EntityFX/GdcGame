/// <reference path="../../../Scripts/typings/angularjs/angular.d.ts" />

namespace Gdcame.Services {
    import HttpService = angular.IHttpService;
    import RequestConfig = angular.IRequestConfig;

    export class UserApiService extends ApiServiceBase implements Services.IUserApiService {
        private apiServiceUri: Gdcame.Services.IUriService;
        private apiAuthServiceAddress: string;

        public constructor($http: HttpService, apiServiceUri : Services.IUriService) {
            super($http);
            this.apiServiceUri = apiServiceUri;
        }

        login(authData: Services.AuthData): angular.IHttpPromise<Services.AuthToken> {
            this.apiAuthServiceAddress = this.apiServiceUri.getApiAddressByLogin(authData.login);
            return this.http(<RequestConfig>{
                method: 'POST',
                url: `${this.apiAuthServiceAddress}/token`,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: (obj : Array<any>) => {
                    var str = [];
                    for (var p in obj)
                        if (obj.hasOwnProperty(p))
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: {
                    grant_type: "password",
                    username: authData.login,
                    password: authData.password
                } as Services.AuthRequestTokenData
            });
        }

        logout(): angular.IHttpPromise<any> {
            return this.http.post(`${this.apiAuthServiceAddress}/api/auth/logout`, {});
        }

        register(user: RegisterData): angular.IHttpPromise<any> {
            return this.http.post(`${this.apiServiceUri.getApiAddressByLogin(user.login)}/api/auth/register/`, user)
                .then((result) => {
                    return result.data;
                });
        }
    }

    angular.module("gdCameApp").service("UserApiService", UserApiService);

    UserApiService.$inject = [
        "$http", "UriService"
    ];
}