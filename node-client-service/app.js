'use strict'
const net = require('net');

var socket = new net.Socket();
socket.connect({
    port: 8000,
    host: 'localhost'
}, () => {
    console.log('Client connected...');
    let bytes = Buffer.from("{\"method\":\"Collect\",\"params\":[1],\"id\":1}");
    let data = Buffer.from([...intToByteArray(bytes.length), ...bytes]);
    socket.write(data);
});

socket.on('data', (data) => {
    console.log(data.toString());
    socket.end();
});
socket.on('error', (err) => {
    console.log(err);
});
socket.on('end', () => {
    console.log('disconnected from server');
});

function intToByteArray(/*long*/integer) {
    // we want to represent the input as a 4-bytes array
    var byteArray = [0, 0, 0, 0];

    for (var index = byteArray.length - 1; index >=0; index--) {
        var byte = integer & 0xff;
        byteArray[index] = byte;
        integer = (integer - byte) / 256;
    }

    return byteArray;
};