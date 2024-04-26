import { Exports } from "./exports";

export default class Accelerometer {
    private exports: Exports;

    constructor(s4e1: Exports) {
        this.handleMotion = this.handleMotion.bind(this);
        this.exports = s4e1;
        window.addEventListener('devicemotion', this.handleMotion);
    }

    private handleMotion(event: DeviceMotionEvent) {
        if(!event.accelerationIncludingGravity) return;

        this.exports.Microsoft.Devices.Sensors.Accelerometer.Invoke(event.accelerationIncludingGravity.x!, event.accelerationIncludingGravity.y!, event.accelerationIncludingGravity.z!);
    }
}