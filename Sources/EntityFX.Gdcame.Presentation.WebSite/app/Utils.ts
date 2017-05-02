namespace Gdcame.Utils {
    export class Utils {
        static getModuloByUserIdHash(userId: string, modulo: number) {
            var value = 0;
            var _2pow4 = 1;
            for (var i = userId.length - 1; i >= 0; i--) {
                var char = userId[i];
                var x = parseInt('0x' + char);
                value = (value + x * _2pow4) % modulo;
                console.log(x + ' ' + value + ' ' + _2pow4);
                _2pow4 = (_2pow4 << 4) % modulo;
            }
            return value;
        }
    }
}