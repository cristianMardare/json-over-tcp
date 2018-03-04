const net = require('net');

var socket = new net.Socket();
socket.connect({
    port: 8000,
    host: 'localhost'
}, () => {
    console.log('Client connected...');
    socket.write("{\"method\":\"Collect\",\"params\":[1],\"id\":1}");
});

socket.on('data', (data) => {
    console.log(data.toString());
    socket.end();
});
socket.on('end', () => {
    console.log('disconnected from server');
});