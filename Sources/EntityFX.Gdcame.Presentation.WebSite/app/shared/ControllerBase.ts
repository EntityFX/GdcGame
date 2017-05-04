namespace Gdcame.Controllers {
    export abstract class ControllerBase {
        public showMessage(type: any, message: String): void {
            alert(message);
        }

        public hideMessage(): void {

        }
    }
}