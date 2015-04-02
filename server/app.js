var crypto = require('crypto'),
    express = require('express'),
    app = express(),
    bodyParser = require('body-parser'),
    multer = require('multer'),
    Map = require('./map');

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(multer());

app.get('/', function(req, res) {
    res.send();
});

// Store rooms as a hash table where the key is a hash of the unique
// id of player1 and player2. Value will be the actual map object
// Might want to move this to redis or some other data store...
var rooms = {};

/**
 * Returns a room for 2 players.
 * Query string
 *     player_1 - unique id for player 1
 *     player_2 - unique id for player 2
 *
 * unique ids should be the same ones that were sent when creating the room
 */
app.get('/rooms', function(req, res) {
    var player1 = req.query.player_1,
        player2 = req.query.player_2,
        hashKey = crypto.createHash('md5').update(player1 + '_' + player2).digest('hex');
    console.log('find room for ' + player1 + ',' + player2 + ' : ' + hashKey);
    if (rooms[hashKey]) {
        res.send(rooms[hashKey]);
    } else {
        res.status(500).send({ error: 'Room does not exist.' });
    }
});

/**
 * Route to create a new room for 2 players
 * POST data:
 *     player_1 - Unique identifier for player 1
 *     player_2 - Unique identifier for player 2
 */
app.post('/rooms', function(req, res) {
    var player1 = req.body.player_1,
        player2 = req.body.player_2,
        hashKey = crypto.createHash('md5').update(player1 + '_' + player2).digest('hex');
    console.log('Creating room for ' + player1 + ',' + player2 + ' @ ' + hashKey);
    rooms[hashKey] = {
        player1: player1,
        player2: player2,
        map: new Map()
    };
    res.json(rooms[hashKey]);
});

var server = app.listen(3000, function() {
    console.log('server loaded');
});
