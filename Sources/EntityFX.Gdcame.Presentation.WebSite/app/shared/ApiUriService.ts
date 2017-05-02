/// <reference path="../../Scripts/typings/js-md5/md5.d.ts" />

namespace Gdcame.Services {
    export class ApiUriService implements IApiUriService {
        private serverIps: string[];
        private apiUri: string;
        private useServerIps: boolean;

        constructor(apiUri: string, serverIps: Array<string>) {
            this.serverIps = serverIps;
            this.apiUri = apiUri;
            this.useServerIps = serverIps != undefined && serverIps.length > 0;
        }

        getApiAddressByLogin(login: string) {
            var userServerNumber = Utils.Utils.getModuloByUserIdHash(md5(login + '_gdcame'), this.serverIps.length);
            return 'http://' + (this.useServerIps ? this.serverIps[userServerNumber] : 'ns' + (userServerNumber + 1) + '.' + this.apiUri);
        }

    }

    angular.module("gdCameApp").service("ApiUriService", ApiUriService);

    ApiUriService.$inject = [
        "apiUri", "serverIps"
    ];
}