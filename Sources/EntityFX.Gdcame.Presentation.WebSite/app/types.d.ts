declare namespace Gdcame.Controllers {
}

declare namespace Gdcame.Services {
    import HttpPromise = angular.IHttpPromise;

    export class AuthData {
        login: string;
        password: string;
    }

    export class RegisterData extends AuthData {
        confirmPassword: string;
    }

    export class AuthToken {
        token_type: string;
        access_token: string;
    }

    export interface IAuthenticateService {
        login(authData: AuthData): HttpPromise<AuthToken>;
        logout(): HttpPromise<any>;
    }

    export interface IUserService extends IAuthenticateService {
        register(user: RegisterData): HttpPromise<any>;
    }

    class AuthRequestTokenData {
        grant_type: string | "password";
        username: string;
        password: string;
    }

    interface IApiUriService {
        getApiAddressByLogin(login: string);
    }
}

