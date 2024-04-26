import { Exports } from "./exports";

const MediaState = {
    Stopped: 0,
    Playing: 1,
    Paused: 2
};

export default class WasmMediaPlayer {
    private volume: number;
    private context: AudioContext;
    private gainNode: GainNode;
    private source: AudioBufferSourceNode | null;
    private exports: any;
    private isGarbage: boolean;

    private audioBuffers: { [key: string]: AudioBuffer } = {};

    constructor(s4e1: Exports) {
        this.volume = 1;
        this.context = new AudioContext();
        this.gainNode = this.context.createGain();
        this.gainNode.connect(this.context.destination);

        this.onEnded = this.onEnded.bind(this);

        this.isGarbage = document.createElement("audio").canPlayType("audio/ogg") === "";

        this.exports = s4e1;
    }

    setVolume(vol: number) {
        this.volume = vol;
        this.gainNode.gain.value = vol;
    }

    getVolume() {
        return this.volume;
    }

    play(path: string, loopPos?: number) {
        this.getAudioBuffer(path)
            .then(audioBuffer => {
                this.playQueue(audioBuffer, loopPos);
            })
    }

    stop() {
        if (this.source) {
            this.source.stop();
            this.source = null;
        }
    }

    preload(path: string) {
        this.getAudioBuffer(path);
    }

    playQueue(buffer: AudioBuffer, loopPos?: number) {
        if (this.source) {
            this.source.removeEventListener("ended", this.onEnded);
            this.source.stop();
        }

        this.source = this.context.createBufferSource();
        this.source.buffer = buffer;
        this.source.connect(this.gainNode);
        this.source.addEventListener("ended", this.onEnded);

        if (loopPos !== undefined) {
            console.log("Setting loop", loopPos, buffer.duration);
            this.source.loopStart = loopPos;
            this.source.loopEnd = buffer.duration;
            this.source.loop = true;
        }

        this.source.start();
        this.setState(MediaState.Playing);
    }

    getAudioBuffer(path: string) {
        // if (this.audioBuffers[path]) {
        //     return Promise.resolve(this.audioBuffers[path]);
        // }

        path = "." + path;
        if (this.isGarbage) {
            path = path.substring(0, path.length - 4) + ".m4a";
        }

        return window.fetch(path)
            .then(response => response.arrayBuffer())
            .then(arrayBuffer => this.context.decodeAudioData(arrayBuffer))
            .then(audioBuffer => {
                this.audioBuffers[path] = audioBuffer;
                return audioBuffer;
            });
    }

    onEnded(e: Event) {
        this.setState(MediaState.Stopped);
    }

    setState(state: typeof MediaState[keyof typeof MediaState]) {
        this.exports.Microsoft.Xna.Framework.Media.WasmMediaPlayer.ChangeState(state);
    }
}
