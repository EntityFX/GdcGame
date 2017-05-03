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

    export class AuthRequestTokenData {
        grant_type: string | "password";
        username: string;
        password: string;
    }

    export interface IApiUriService {
        getApiAddressByLogin(login: string): string;
        setApiAddressByLogin(login:string): void;
    }

    export interface IGameApiService {
        getGameData(): HttpPromise<GameData>;
        getCounters(): HttpPromise<Cash>;
        performManualStep(verificationNumber: number): HttpPromise<ManualStepResult>;
        buyFundDriver(fundDriverId: number): HttpPromise<BuyItem>;
        fightAgainstInflation(): HttpPromise<Cash>;
        activateDelayedCounter(counterId: number): HttpPromise<Cash>;
    }

    export class CounterBase {
        Id: number;
        Name: string;
        Value: number;
        Type: number;
    }

    export class Cash {
        OnHand: number;
        TotalEarned: number;
        Counters: Array<CounterBase>;
    }

    export class GenericCounter extends CounterBase {
        Bonus: number;
        BonusPercentage: number;
        Inflation: number;
        SubValue: number;
    }

    export class DelayedCounter extends CounterBase {
        SecondsRemaining: number;
        UnlockValue: number;
    }

    export class Item {
        Id: number;
        Name: string;
        Price: number;
        InflationPercent: number;
        UnlockBalance: number;
        IsUnlocked: boolean;
        Bought: number;
        Picture: string;
        Incrementors: Array<string>;
    }

    export class GameData {
        Items: Array<Item>;
        Cash: Cash;
    }

    export class ManualStepResult {
        VerificationData: VerificationData;
        ModifiedCountersInfo: Cash;
    }

    export class VerificationData {
        FirstNumber: number;
        SecondNumber: number;
    }

    export class BuyItem {
        ItemBuyInfo: Item;
        ModifiedCountersInfo: Cash;
    }
}

