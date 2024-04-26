export interface Instance {
    Module: {
        FS: any;
        IDBFS: any;
        canvas: any;
        setMainLoop: (cb: any) => void;
    };
}

export interface DotNet {
    withDiagnosticTracing: (enabled: boolean) => DotNet;
    withApplicationArgumentsFromQuery: () => DotNet;
    create: () => Promise<{
        setModuleImports: (name: string, imports: any) => void;
        getAssemblyExports: (name: string) => Promise<any>;
        getConfig: () => any;
        instance: Instance;
    }>;

    instance: Instance;
    run: () => Promise<void>;
}
export const dotnet: DotNet;
