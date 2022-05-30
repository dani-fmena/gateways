import request from "supertest";
import { dateTimeRegex } from "../lib/rexpr";
import { gatewayPostOutSchema, gatewayRowSchema } from "../lib/schemas";

//#region ======= VARS DECLARATION ====================================================

const jsonEd = require("edit-json-file");
const f = jsonEd('./kript.json');
const scopedUrl = f.data.url + "/mngmt/acgateway"               // getting server base url for scoping the test case url

let Ajv = require('ajv');                                       // JSON Schema validator
const addFormats = require("ajv-formats")                       // Ajv formats

const rowGatewayId = 2                                          // endpoint request param


/**
 * Gateway object to test a gateway creation.
 * @type {{serialNumber: string, name: string, ipAddress: string}}
 */
const gatewayOk = {
    "name": "Avastanza",
    "serialNumber": "ABC789",
    "ipAddress": "10.14.200.2"
}

/**
 * Gateway object to test a bad request. The IPv4 field is wrong.
 * @type {{serialNumber: string, name: string, ipAddress: string}}
 */
const gatewayInvalidFields = {     
    "name": "Office",
    "serialNumber": "ABC789",
    "ipAddress": "256.256.255.0"
}

/**
 * For testing the update gateway endpoint with non existing gateway
 * @type {{serialNumber: string, name: string, ipAddress: string, id: number}}
 */
let nonExistingGateway = {
    "id": 15,
    "name": "Chese",
    "serialNumber": "OOOOOOOO",
    "ipAddress": "20.40.200.10"
}

//#endregion ==========================================================================

//#region ======= SCHEMA SETUP ========================================================

let ajv = new Ajv();

addFormats(ajv, ["email"])
ajv.addFormat('date-time', {
    validate: ( dateTimeString ) => dateTimeRegex.test(dateTimeString)
});

let gatewayRowSchemaValidator = ajv.compile(gatewayRowSchema);
let gatewayPostSchemaValidator = ajv.compile(gatewayPostOutSchema);

//#endregion ==========================================================================

//#region ======= TEST CASES ==========================================================

beforeAll(async () => {
    await new Promise((r) => setTimeout(r, 1000));
});

/**
 * Get a Gateway row
 */
describe(`GATEWAY [GET] ${scopedUrl}/${rowGatewayId}`, () => {

    test('chk 404', async () => {
        const response = await request(scopedUrl)
            .get(`/10000`)            

        expect(response.statusCode).toEqual(404);
    });

    test('chk get GATEWAY data and it match the GATEWAY schema', async () => {
        const response = await request(scopedUrl)
            .get(`/rows/${rowGatewayId}`)            

        expect(response.statusCode).toEqual(200);
        
        let valid = gatewayRowSchemaValidator(response.body);
        expect(valid).toBeTruthy();
    });
})

/**
 * Get a Gateway row list
 */
describe(`GATEWAY list [GET] ${scopedUrl}/rows`, () => {
    
    test('chk gateway list amount', async () => {
        const response = await request(scopedUrl)
            .get('/rows')

        expect(response.statusCode).toEqual(200);
        expect(response.body.length).toBeGreaterThanOrEqual(2);
    });

    test('chk GATEWAY match the schema for a list', async () => {
        const response = await request(scopedUrl)
            .get('/rows')

        expect(response.statusCode).toEqual(200);
        let valid = gatewayRowSchemaValidator(response.body[0]);
        expect(valid).toBeTruthy();
    });
})

/**
 * Delete existing Gateways
 */
describe(`GATEWAY [DELETE] ${scopedUrl}/batch`, () => {

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
 * Create a new Gateway
 */
describe(`GATEWAY [POST] ${scopedUrl}`, () => {

    test('chk 400 with invalid params', async () => {

        const reponse = await request(scopedUrl)
            .post('')
            .send(gatewayInvalidFields)
            .set('Content-Type', 'application/json')

        expect(reponse.statusCode).toEqual(400);
        expect(reponse.body.errors['IpAddress'][0]).toContain('don\'t seems to be valid');
    });

    test('chk create new gateway and the response match the scheme', async () => {
        const response = await request(scopedUrl)
            .post('')
            .send(gatewayOk)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(201);

        let valid = gatewayPostSchemaValidator(response.body);
        expect(valid).toBeTruthy();

        expect(response.body.id >= 0).toBeTruthy();
        expect(response.body.name).toEqual(gatewayOk.name);
        expect(response.body.ipAddress).toEqual(gatewayOk.ipAddress);
    });
});

/**
 * Update an existing GATEWAY
 */
describe(`Gateway [PUT] ${scopedUrl}`, () => {

    test('chk 400 with invalid params', async () => {

        const reponse = await request(scopedUrl)
            .post('')
            .send(gatewayInvalidFields)
            .set('Content-Type', 'application/json')

        expect(reponse.statusCode).toEqual(400);
        expect(reponse.body.errors['IpAddress'][0]).toContain('don\'t seems to be valid');
    });

    test('chk 404', async () => {
        
        const response = await request(scopedUrl)
            .put('')
            .send(nonExistingGateway)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(404);
    });

    test('chk update an existing gateway and response match the schema', async () => {

        nonExistingGateway.id = 10              // Now it should be an 'Existing Gateway'

        const response = await request(scopedUrl)
            .put('')
            .send(nonExistingGateway)
            .set('Content-Type', 'application/json')

        expect(response.statusCode).toEqual(200);

        let valid = gatewayPostSchemaValidator(response.body);
        expect(valid).toBeTruthy();
        
        expect(response.body.serialNumber).toEqual(nonExistingGateway.serialNumber);
        expect(response.body.name).toEqual(nonExistingGateway.name);
    });

});

//#endregion ==========================================================================
