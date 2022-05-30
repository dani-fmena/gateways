require('colors');

const jsonEd = require("edit-json-file");
const os = require("os");
const fs = require('fs')
const { execSync, spawn } = require("child_process");

const factoryOpts = {
    sync: true,
    args: "",
    envs: undefined,
    callback: function ( data ) {
        console.log(data.yellow);
    }
}

//#region ======= SHARED VARS =========================================================

const f = jsonEd('./kript.json');
var baseUrl;

//#endregion ==========================================================================

function buildFactory( project ) {
    try {
        console.log(`[+] BUILDING ${project} [DB Seeders]...`.blue);
        execSync(`dotnet build ${project}`);
        console.log("[+] BUILD COMPLETE !".green);
    } catch ( err ) {
        console.log(err.stdout.toString());
        throw `[-] BUILD of ${project} FAILED !`.red;
    }
}

function runFactory( project, options = {
    args: undefined,
    sync: undefined,
    envs: undefined,
    callback: undefined
} ) {
    let command = `dotnet run --project ${project} --no-build `;

    if ( options.envs ) {
        const envString = Object.keys(options.envs)
            .map(key => `${key}=${options.envs[key]}`)
            .join(" ");
        if ( os.platform() === "win32" ) command = "set " + envString + " " + command;
        else command = "export " + envString + " " + command;
    }
    if ( options.args ) {
        command += `-- ${options.args}`;
    }
    if ( options.sync ) {
        try {
            console.log("[+] TRYING TO CLEAN, MIGRATE AND SEED DB...".blue);
            execSync(command);
            console.log("[+] DB OPS COMPLETE !".green);
            return {}
        } catch ( err ) {
            console.log(err.stdout.toString());
            throw `[-] DB OPS FAILED !`.red;
        }
    } else {
        const proc = spawn("dotnet", [ "run", `--project ${project}`, "--no-build" ], {
            env: options.envs ? options.envs : process.env,
        });
        proc.stdout.on('data', ( data ) => {
            options.callback(data);
        });
        proc.stderr.on('data', ( d ) => console.log(d));
        return proc;
    }
}

async function doJestTests() {
    
    //#region ======= SETUP ===============================================================

    // some bootstrapping vars
    const config = require('./envs/config.json');

    baseUrl = `http://${config.HOST}:${config.PORT}/v${config.API_VERSION}`;
    f.set("url", baseUrl);
    
    //#endregion ==========================================================================
    
    //#region ======= CALLING TESTS =======================================================

    // in debug mode we configure jest to wait 5 minutes tops. In the other hand, jest will be wait 1 minute
    // using normal mode (debug == false).
    
    try {
        console.log("[+] RUNNING TESTS...".blue);        
        execSync(`jest --verbose --colors --bail --testTimeout=${config.DEBUG ? '300000' : '60000' }`, { stdio: 'inherit' }); 
        console.log("[+] TESTS COMPLETE !".green);
    } catch ( err ) {        
        console.error("[-] TESTS FAILED !".red)
    }
    
    //#endregion ==========================================================================

}

async function runTests() {
    
    buildFactory("../gateway.factory");                                   // building factory (seeders) project
    runFactory("../gateway.factory", factoryOpts);                        // once running, cleaning the database and seed / populate
    await doJestTests()                                                           // calling the test
}

(async () => {await runTests();})().catch();
