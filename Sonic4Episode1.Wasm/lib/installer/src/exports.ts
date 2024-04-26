
export type Exports = {
    Microsoft: {
        Xna: {
            Framework: {
                Media: {
                    WasmMediaPlayer: {
                        ChangeState: (state: number) => void;
                    };
                };
            };
        },
        Devices: {
            Sensors: {
                Accelerometer: {
                    Invoke: (x: number, y: number, z: number) => void;
                }
            }
        }
    };
};
