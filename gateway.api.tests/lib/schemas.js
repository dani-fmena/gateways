/**
 * Represent a gateway record DTO from the backend 
 * @type {{additionalProperties: boolean, type: string, properties: {serialNumber: {type: string}, peripheralsAssociated: {type: string, minimum: number}, name: {type: string}, ipAddress: {minLength: number, pattern: string, type: string, maxLength: number}, id: {type: string, minimum: number}}, required: string[]}}
 */
export const gatewayRowSchema = {
    type: 'object',
    properties: {
        id: {
            type: 'integer',
            minimum: 1
        },
        name: { type: 'string' },
        serialNumber: { type: 'string' },
        ipAddress: {
            type: 'string',
            minLength: 7,
            maxLength: 15,
            pattern: "^((25[0-5]|(2[0-4]|1[0-9]|[1-9]|)[0-9])(\.(?!$)|$)){4}$"
        },
        peripheralsAssociated: { 
            type: 'integer',
            minimum: 0 
        },
    },
    required: [ 'id', 'name', 'serialNumber', 'ipAddress', 'peripheralsAssociated' ],
    additionalProperties: true
}

/**
 * * Represent a gateway response
 * @type {{additionalProperties: boolean, type: string, properties: {serialNumber: {type: string}, name: {type: string}, ipAddress: {minLength: number, pattern: string, type: string, maxLength: number}, id: {type: string, minimum: number}}, required: string[]}}
 */
export const gatewayPostOutSchema = {
    type: 'object',
    properties: {
        id: {
            type: 'integer',
            minimum: 1
        },
        name: { type: 'string' },
        serialNumber: { type: 'string' },
        ipAddress: {
            type: 'string',
            minLength: 7,
            maxLength: 15,
            pattern: "^((25[0-5]|(2[0-4]|1[0-9]|[1-9]|)[0-9])(\.(?!$)|$)){4}$"
        },
    },
    required: [ 'id', 'name', 'serialNumber', 'ipAddress'],
    additionalProperties: true
}