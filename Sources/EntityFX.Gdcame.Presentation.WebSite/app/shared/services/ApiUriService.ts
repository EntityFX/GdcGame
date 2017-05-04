/// <reference path="../../../Scripts/typings/js-md5/md5.d.ts" />

namespace Gdcame.Services {
    export class UriService implements Gdcame.Services.IUriService {
        private serverIps: string[];
        private apiUri: string;
        private useServerIps: boolean;
        private localStorage: angular.storage.IStorageService;

        constructor($localStorage: angular.storage.IStorageService, apiUri: string, serverIps: Array<string>) {
            this.serverIps = serverIps;
            this.apiUri = apiUri;
            this.useServerIps = serverIps != undefined && serverIps.length > 0;
            this.localStorage = $localStorage;
        }

        getApiAddressByLogin(login: string) {
            var userServerNumber = Utils.Utils.getModuloByUserIdHash(md5(login + '_gdcame'), this.serverIps.length);
            return 'http://' + (this.useServerIps ? this.serverIps[userServerNumber] : 'ns' + (userServerNumber + 1) + '.' + this.apiUri);
        }

        getSesionApiAddress(): string {
            return this.localStorage["globals"].apiAddress;
        }
    }

    angular.module("gdCameApp").service("UriService", UriService);

    UriService.$inject = [
        "$localStorage", "apiUri", "serverIps"
    ];
}