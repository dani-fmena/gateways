/* This file if for writing custom matchers */

expect.extend({
    toBeISODate( received ) {

        // This regexp checks for formatting
        if ( !/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z/.test(received) ) {
            return {
                pass: false,
                message: `Expected ${received} to be a valid ISO date string`,
            };
        }

        // This checks if JS can correctly parse it
        const d = new Date(received);
        return d.toISOString() === received
            ? {
                pass: true,
                message: 'Expected ${received} not to be a valid ISO date string',
            }
            : {
                pass: false,
                message: `Expected ${received} to be a valid ISO date string`,
            };
    },
});

// This sample takes one argument
expect.extend({
    toBePowerOf( received, power ) {
        if ( typeof power !== 'number' ) {
            throw new Error('expected power to be a number');
        }

        if ( typeof received !== 'number' ) {
            throw new Error('expected value to be a number');
        }

        let receivedCopy = received;
        while ( receivedCopy % power === 0 ) receivedCopy = receivedCopy / power;

        return receivedCopy === 1 ? ({
            pass: true,
            message: `Expected ${received} not to be a power of ${power}`
        }) : ({
            pass: false,
            message: `Expected ${received} to be a power of ${power}`
        });
    }
});