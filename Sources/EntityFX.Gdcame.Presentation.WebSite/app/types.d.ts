declare namespace Gdcame.Controllers {
}

declare namespace Gdcame.Services {
    import Promise = angular.IPromise;

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

    export interface IAuthenticateApiService {
        login(authData: AuthData): angular.IHttpPromise<AuthToken>;
        logout(): angular.IHttpPromise<any>;
    }

    export interface IAuthenticateDataService {
        login(authData: AuthData): angular.IPromise<AuthToken>;
        logout(): angular.IPromise<any>;
    }

    export interface ISessionService {
        getLogin(): string;
    }

    export interface IRegisterApiService {
        register(user: RegisterData): angular.IHttpPromise<any>;
    }

    export interface IRegisterDataService {
        register(user: RegisterData): angular.IPromise<any>;
    }

    export interface IUserApiService extends IAuthenticateApiService, IRegisterApiService {
    }

    export class AuthRequestTokenData {
        grant_type: string | "password";
        username: string;
        password: string;
    }

    export interface IUriService {
        getApiAddressByLogin(login: string): string;
        getSesionApiAddress(): string;
    }

    export interface IGameApiService {
        getGameData(): angular.IHttpPromise<GameData>;
        getCounters(): angular.IHttpPromise<Cash>;
        performManualStep(verificationNumber: number): angular.IHttpPromise<ManualStepResult>;
        buyFundDriver(fundDriverId: number): angular.IHttpPromise<BuyItem>;
        fightAgainstInflation(): angular.IHttpPromise<Cash>;
        activateDelayedCounter(counterId: number): angular.IHttpPromise<Cash>;
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

    export interface IAdminApiService {
        getStatistics(serverAddress: string): angular.IHttpPromise<ServerStatisticsInfo>;
        getActiveSessions(serverAddress: string): angular.IHttpPromise<Array<UserSessions>>;
    }

    export class ServerStatisticsInfo {
        ActiveSessionsCount: number;
        ActiveGamesCount: number;
        RegistredUsersCount: number;
        ServerUptime: string;
        ServerStartDateTime: string;
        PerformanceInfoModel: PerformanceInfo;
        ResourcesUsageInfoModel: ResourcesUsageInfo;
        SystemInfoModel: SystemInfo;
        ActiveWorkers: Array<string>;
    }

    export class PerformanceInfo {
        CalculationsPerCycle: string;
        PersistencePerCycl: string;
    }

    export class ResourcesUsageInfo {
        MemoryAvailable: number;
        MemoryUsedByProcess: number;
        CpuUsed: number;
    }

    export class SystemInfo {
        Runtime: string;
        Os: string;
        CpusCount: number;
        MemoryTotal: number;
    }

    export class UserSessions {
        Login: string;
        Sessions: Array<SessionInfo>;
    }

    export class SessionInfo {
        SessionIdentifier: string;
        LastActivity: string;
    }

    export interface IRatingApiService {
        getRaiting(serverAddress: string, top: number): angular.IHttpPromise<TopRatingStatisticsModel>;
    }

    export class TopRatingStatisticsModel {
        ManualStepsCount: TopStatisticsAggregate;
        TotalEarned: TopStatisticsAggregate;
        RootCounter: TopStatisticsAggregate;
    }

    export class TopStatisticsCounter {
        Login: string;
        UserId: string;
        Value: number;
    }

    export class TopStatisticsAggregate {
        Day: Array<TopStatisticsCounter>;
        Week: Array<TopStatisticsCounter>;
        Total: Array<TopStatisticsCounter>;
    }
}

