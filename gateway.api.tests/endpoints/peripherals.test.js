import request from "supertest";
import { dateTimeRegex } from "../lib/rexpr";
import { peripheralRowSchema, gatewayRowSchema } from "../lib/schemas";

//#region ======= VARS DECLARATION ====================================================

const jsonEd = require("edit-json-file");
const f = jsonEd('./kript.json');
const scopedUrl = f.data.url + "/mngmt/acperipheral"            // getting server base url for scoping the test case url

let Ajv = require('ajv');                                       // JSON Schema validator
const addFormats = require("ajv-formats")                       // Ajv formats

const rowPeripheralId = 6                                       // endpoint request param


/**
 * Peripheral object to test a gateway creation.
 * @type {{vendor: string, isOnline: boolean, id: number, gatewayId: number}}
 */
let peripheralOk = {
    "id": 15,
    "vendor": "Yamato",
    "isOnline": false,
    "gatewayId": 1
}

/**
 * Peripheral object to test a bad request. The IPv4 field is wrong.
 * @type {{vendor: string, isOnline: boolean, gatewayId: string}}
 */
const peripheralInvalidFields = {    
    "vendor": "Macintosh",
    "isOnline": true,
    "gatewayId": "Rasengan"
}

/**
 * For testing the update peripheral endpoint with non existing gateway
 * @type {{vendor: string, isOnline: boolean, id: number, gatewayId: string}}
 */
let nonExistingPeripheral = {
    "id": 500,
    "vendor": "Macintosh",
    "isOnline": true,
    "gatewayId": "1"
}

/**
 * For testing the update peripheral endpoint
 * @type {{uid: string, vendor: string, isOnline: boolean, id: number, gatewayId: string}}
 */
let updatePeripheral = {
    "id": 1,
    "uid": "6C2FF3C0-A4DB-1989-B596-1492167405CC",
    "vendor": "Macintosh",
    "isOnline": true,
    "gatewayId": "1"
}

//#endregion ==========================================================================

//#region ======= SCHEMA SETUP ========================================================

let ajv = new Ajv();

addFormats(ajv, ["email"])
ajv.addFormat('date-time', {
    validate: ( dateTimeString ) => dateTimeRegex.test(dateTimeString)
});

let peripheralSchemaValidator = ajv.compile(peripheralRowSchema);

//#endregion ==========================================================================

//#region ======= TEST CASES ==========================================================

beforeAll(async () => {
    await new Promise((r) => setTimeout(r, 1000));
});

/**
 * Get a peripheral row
 */
describe(`PERIPHERAL [GET] ${scopedUrl}/${rowPeripheralId}`, () => {

    test('chk 404', async () => {
        const response = await request(scopedUrl)
            .get(`/10000`)            

        expect(response.statusCode).toEqual(404);
    });

    test('chk get peripheral data and it match the peripheral schema', async () => {
        const response = await request(scopedUrl)
            .get(`/rows/${rowPeripheralId}`)            

        expect(response.statusCode).toEqual(200);
        
        let valid = peripheralSchemaValidator(response.body);
        expect(valid).toBeTruthy();
    });
})

/**
 * Get a Peripheral row list according to a given gateway 
 */
describe(`PERIPHERAL list [GET] for a given GATEWAY  ${scopedUrl}/rows/gateway/${rowPeripheralId}`, () => {

    test('chk peripheral list amount', async () => {
        const response = await request(scopedUrl)
            .get( `/rows/gateway/${rowPeripheralId}`)

        expect(response.statusCode).toEqual(200);
        expect(response.body.length).toBeGreaterThanOrEqual(1);
    });

    test('chk peripheral match the schema for a list', async () => {
        const response = await request(scopedUrl)
            .get( `/rows/gateway/${rowPeripheralId}`)

        expect(response.statusCode).toEqual(200);
        let valid = peripheralSchemaValidator(response.body[0]);
        expect(valid).toBeTruthy();
    });
})

/**
 * Get a PERIPHERAL row list
 */
describe(`PERIPHERAL list [GET] ${scopedUrl}/rows`, () => {
    
    test('chk peripheral list amount', async () => {
        const response = await request(scopedUrl)
            .get('/rows')

        expect(response.statusCode).toEqual(200);
        expect(response.body.length).toBeGreaterThanOrEqual(10);
    });

    test('chk peripheral match the schema for a list', async () => {
        const response = await request(scopedUrl)
            .get('/rows')

        expect(response.statusCode).toEqual(200);
        let valid = peripheralSchemaValidator(response.body[0]);
        expect(valid).toBeTruthy();
    });
})

/**
 * Delete existing Gateways
 */
describe(`PERIPHERAL [DELETE] ${scopedUrl}/batch`, () => {

    test('chk 400 invalid params', async () => {

        const response = await request(scopedUrl)
            .delete('/batch')
            .send([ "asdf", "qwerty" ])
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(400);
    });

    test('chk 404 for nonexistent gateway', async () => {
        const response = await request(scopedUrl)
            .delete('/batch')
            .send([800])
            .set('Content-Type', 'application/json');

        expect(response.statusCode).toEqual(404);
    });

    test('chk delete a batch of gateways, also check 404 for just deleted stores', async () => {

        const response = await request(scopedUrl)
            .delete('/batch')
            .send([ 3, 4 ])
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(204);

        // check 404 for just deleted stores
        const resDel = await request(scopedUrl)
            .delete('/batch')
            .send([ 3, 4 ])
            .set('Content-Type', 'application/json')
        
        expect(resDel.statusCode).toEqual(404);
    });
});

/**
 * Create a new PERIPHERAL
 */
describe(`PERIPHERAL [POST] ${scopedUrl}`, () => {

    test('chk 400 with invalid params', async () => {

        const response = await request(scopedUrl)
            .post('')
            .send(peripheralInvalidFields)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(400);
        expect(response.body.errors['gatewayId'][0]).toContain('Could not convert string to integer');
    });

    test('chk 400 with invalid ID (greater than 0)', async () => {

        const response = await request(scopedUrl)
            .post('')
            .send(peripheralOk)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(400);
        peripheralOk.id = 0;
    });

    test('chk create new peripheral and the response match the scheme', async () => {
        const response = await request(scopedUrl)
            .post('')
            .send(peripheralOk)
            .set('Content-Type', 'application/json')
        
        // normal post case
        if (response.statusCode === 201) {
            expect(response.statusCode).toEqual(201);

            let valid = peripheralSchemaValidator(response.body);
            expect(valid).toBeTruthy();

            expect(response.body.id >= 0).toBeTruthy();
            expect(response.body.vendor).toEqual(peripheralOk.vendor);
            expect(response.body.isOnline).toEqual(peripheralOk.isOnline);
        }
        else {
            
            // this may happen,´cause is a business rules case: a gateway can't have more than 10 peripherals associated
            expect(response.statusCode).toEqual(400);
            expect(response.body.title).toContain('The gateway has more than 10 peripherals');
        }
        
    });

    test('chk 400 a business rules case: a gateway can\'t have more than 10 peripherals associated', async () => {
        
        let i = 10;
        let peripheral =  {
            "vendor": "Macintosh",
            "isOnline": true,
            "gatewayId": "8"
        }
        let responseCode = 0;

        
        // keeping posting 'til backend says 400
        do {
            const response = await request(scopedUrl)
                .post('')
                .send(peripheral)
                .set('Content-Type', 'application/json')
            i--;

            responseCode = response.statusCode;
        }
        while (responseCode !== 400 || i >= 0);

        expect(responseCode).toEqual(400);
    });
});

/**
 * Update an existing PERIPHERAL
 */
describe(`Gateway [PUT] ${scopedUrl}`, () => {

    test('chk 400 with invalid params', async () => {
        const response = await request(scopedUrl)
            .post('')
            .send(peripheralInvalidFields)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(400);
        expect(response.body.errors['gatewayId'][0]).toContain('Could not convert string to integer');
    });

    test('chk 404', async () => {

        const response = await request(scopedUrl)
            .put('')
            .send(nonExistingPeripheral)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(404);
    });

    test('chk 400 \'cause non existing associated gateway in the request data', async () => {

        nonExistingPeripheral.gatewayId = 800;

        const response = await request(scopedUrl)
            .put('')
            .send(nonExistingPeripheral)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(400);
        expect(response.body.title).toContain('or the gateway doesn\'t exist');
    });
    
    test('chk update an existing gateway and response match the schema', async () => {
        
        const response = await request(scopedUrl)
            .put('')
            .send(updatePeripheral)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(200);

        let valid = peripheralSchemaValidator(response.body);
        expect(valid).toBeTruthy();

        expect(response.body.vendor).toEqual(updatePeripheral.vendor);
        expect(response.body.isOnline).toEqual(updatePeripheral.isOnline);
        expect(response.body.uid).toEqual(updatePeripheral.uid.toLowerCase());
    });

});

//#endregion ==========================================================================
