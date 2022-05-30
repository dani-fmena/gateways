import request from "supertest";
import { dateTimeRegex } from "../lib/rexpr";
import { gatewayDataSchema, storePageSchema } from "../lib/schemas";

//#region ======= VARS DECLARATION ====================================================

const jsonEd = require("edit-json-file");
const f = jsonEd('./kript.json');
const scopedUrl = f.data.url + "/mngmt/acgateway"               // getting server base url for scoping the test case url

let Ajv = require('ajv');                                       // JSON Schema validator
const addFormats = require("ajv-formats")                       // Ajv formats

const rowGatewayId = 2                                          // endpoint request param

const sStoreOk = {
    "storeActor": {
        // "id": 0,
        "isBan": false,
        "cell": "52004010",
        "name": "Naruto",
        "lastName": "Uzumaki",
        "email": "n.uzumaki@gmail.com",
        "password": "h1n4t4shan",
        "passwordConfirmation": "h1n4t4shan",
        "countryCode": "JP",
        "stateCode": "OS",
        "address": "Agatsumi .St #427"
    },
    "storeData": {
        // "accountId": 0,
        "isApproved": true,
        "isActive": false,
        "storeName": "Isharaku Ramen",
        "storeType": 1,
        "priceLevel": 3,
        "readyTime": 30,
        "long": -82.35896,
        "lat": 23.135305
    }
}

const sStore_InvalidFields = {
    "storeActor": {
        // "id": 0,
        "isBan": 400,
        "cell": "rasengan",
        "name": 4500,
        "lastName": 4500,
        "email": "n.uzumaki@gmail",
        "password": "h1n4t4",
        "passwordConfirmation": "h1n4t4",
        "countryCode": 750,
        "stateCode": "OS",
        "address": "Agatsumi .St #427Agatsumi .St #427Agatsumi .St #427Agatsumi .St #427Agatsumi .St #427Agatsumi .St #427"
    },
    "storeData": {
        // "accountId": 0,
        "isApproved": true,
        "isActive": false,
        "storeName": false,
        "storeType": 3,
        "priceLevel": 40,
        "readyTime": "Hokage",
        "long": -82.35896,
        "lat": 23.135305
    }
}

const sStore_Update = {
    "storeActor": {
        "id": 0,
        "isBan": false,
        "cell": "52247780",
        "name": "Inna",
        "lastName": "Doe",
        "email": "myuser@gmail.com",
        "password": "mys3cr3t3",
        "passwordConfirmation": "mys3cr3t3",
        "countryCode": "CU",
        "stateCode": "LH",
        "address": "Agatsumi .St #427"
    },
    "storeData": {
        "accountId": 0,
        "isApproved": true,
        "isActive": false,
        "storeName": "Mama mia",
        "storeType": 1,
        "priceLevel": 1,
        "readyTime": 60,
        "long": 20.11111,
        "lat": 40.111111
    }
}

//#endregion ==========================================================================

//#region ======= SCHEMA SETUP ========================================================

let ajv = new Ajv();

addFormats(ajv, ["email"])
ajv.addFormat('date-time', {
    validate: ( dateTimeString ) => dateTimeRegex.test(dateTimeString)
});

// let pageSchemaValidator = ajv.compile(storePageSchema);
let dataSchemaValidator = ajv.compile(gatewayDataSchema);

//#endregion ==========================================================================

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
        
        let valid = dataSchemaValidator(response.body);
        expect(valid).toBeTruthy();
    });
})

/**
 * Get a Gateway row list
 */
describe(`GATEWAY list [GET] ${scopedUrl}/rows`, () => {
    
    test('chk GATEWAY list amount', async () => {
        const response = await request(scopedUrl)
            .get('/rows')

        expect(response.statusCode).toEqual(200);
        expect(response.body.length).toBeGreaterThanOrEqual(2);
    });

    test('chk GATEWAY match the schema for a list', async () => {
        const response = await request(scopedUrl)
            .get('/rows')

        expect(response.statusCode).toEqual(200);
        let valid = dataSchemaValidator(response.body[0]);
        expect(valid).toBeTruthy();
    });
})

/**
 * Delete existing gateways
 */
describe(`GATEWAY [DELETE] ${scopedUrl}/batch`, () => {

    test('chk 400 invalid params', async () => {

        const response = await request(scopedUrl)
            .delete('/bulk')
            .send([ "asdf", "qwerty" ])
            .set('Content-Type', 'application/json')
            .set('Authorization', 'Bearer ' + authTk)

        expect(response.statusCode).toEqual(400);
    });

    test('chk 401', async () => {
        const response = await request(scopedUrl)
            .delete('/bulk')
            .send([3])
            .set('Content-Type', 'application/json');

        expect(response.statusCode).toEqual(401);
    });

    test('chk 404 for nonexistent store', async () => {
        const response = await request(scopedUrl)
            .delete('/bulk')
            .send([800])
            .set('Authorization', 'Bearer ' + authTk)
            .set('Content-Type', 'application/json');

        expect(response.statusCode).toEqual(404);
    });

    test('chk delete a batch of stores, also check 404 for already deleted stores', async () => {

        const response = await request(scopedUrl)
            .delete('/bulk')
            .send([ 3, 4 ])
            .set('Content-Type', 'application/json')
            .set('Authorization', 'Bearer ' + authTk)

        expect(response.statusCode).toEqual(204);

        // check 404 for already deleted stores
        const resDel = await request(scopedUrl)
            .delete('/bulk')
            .send([ 3, 4 ])
            .set('Content-Type', 'application/json')
            .set('Authorization', 'Bearer ' + authTk)

        expect(resDel.statusCode).toEqual(404);
    });

});
