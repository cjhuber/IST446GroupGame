var colors = require('colors'),
    _ = require('lodash'),
    fs = require('fs');

var Tile = require('./tile'),
    Rect = require('./rect');

// Map properties
var MAX_WIDTH = 50,
    MAX_HEIGHT = 50,
    MAX_ROOM_SIZE = 10,
    MIN_ROOM_SIZE = 6,
    MAX_ROOMS = 15,
    MIN_ROOMS = 5;

// Terrain values
var GROUND = '0',
    WALL = '1';

var Map = function() {
    this.terrain = [];
    this.rooms = [];
    this.roomCount = 0;

    // Start with a terrain that is all walls, then start carving rooms and tunnels
    for (var x = 0; x < MAX_WIDTH; x++) {
        this.terrain[x] = [];
        for (var y = 0; y < MAX_HEIGHT; y++) {
            this.terrain[x][y] = new Tile(WALL);
        }
    }

    this.initTerrain();

};

/**
 * Generates the map's terrain array
 */
Map.prototype.initTerrain = function() {
    var roomCount = Math.floor(Math.random() * (MAX_ROOMS - MIN_ROOMS) + MIN_ROOMS),
        prevX = 0,
        prevY = 0;

    for (var r = 0; r < roomCount; r++) {

        // Keep generating rooms until a valid one is found
        var isValidRoom = false,
            newRoom;
        while (!isValidRoom) {
            newRoom = this.generateRoom();
            isValidRoom = true;
            for (var i = 0; i < this.rooms.length; i++) {
                if (newRoom.intersects(this.rooms[i])) {
                    isValidRoom = false;
                    i = this.rooms.length;
                }
            }
        }

        if (isValidRoom) {
            this.createRoom(newRoom);

            // Connect to previously created room (if there is a previous room)
            if (this.rooms.length == 0) {
                // Make first room the room that the player starts in
            } else {

                var prevRoom = this.rooms[this.rooms.length - 1];
                console.log(newRoom.center());
                console.log(prevRoom.center());

                var hv = Math.round(Math.random());
                if (hv === 1) {
                    this.createHTunnel(prevRoom.center().y, newRoom.center().y, prevRoom.center().x);
                    // Add extra horizontal tunnel to vary widths of tunnnels, and add tunnels that leads to a dead end
                    // Due to varyng widts of the tunnels, causes rooms not to be perfect rectangles
                    this.createHTunnel(prevRoom.center().y, newRoom.center().y, prevRoom.center().x + 1);
                    this.createVTunnel(prevRoom.center().x, newRoom.center().x, newRoom.center().y);
                } else {
                    this.createVTunnel(prevRoom.center().x, newRoom.center().x, prevRoom.center().y);
                    this.createHTunnel(prevRoom.center().y, newRoom.center().y, newRoom.center().x);
                    this.createHTunnel(prevRoom.center().y, newRoom.center().y, prevRoom.center().x + 1);
                }
            }
            this.rooms.push(newRoom);

        }
    }

};

/**
 * Creates a room given a Rect object
 * @param {Rect} room - Rect object with start x, y, width, and height
 */
Map.prototype.createRoom = function(room) {
    console.log('Creating room');
    console.log(room);
    for (var x = room.x1; x < room.x2; x++) {
        for (var y = room.y1; y < room.y2; y++) {
            this.terrain[x][y].type = GROUND;
        }
    }
};

/**
 * Creates a horizontal tunnel at a coordinate
 * @param  {int} x1 - Start x position
 * @param  {int} x2 - end x position
 * @param  {y} y - start y position
 */
Map.prototype.createHTunnel = function(x1, x2, y) {
    var start = Math.min(x1, x2),
        end = Math.max(x1, x2);

    console.log('creating horizontal tunnel [' + start + ', ' + y + '] -> [' + end + ', ' + y + ']');
    for (var x = start; x < end + 1; x++) {
        this.terrain[y][x].type = GROUND;
    }
};


/**
 * Creates a vertical tunnel at a coordinate
 * @param  {int} y1 - Start y position
 * @param  {int} y2 - Ending y position
 * @param  {x} - Width of tunnel
 */
Map.prototype.createVTunnel = function(y1, y2, x) {
    var start = Math.min(y1, y2),
        end = Math.max(y1, y2);

    console.log('creating vertical tunnel ' + start + ' -> ' + end + ' [' + x + ',' + y1 + ']');
    for (var y = start; y < end; y++) {
        this.terrain[y][x].type = GROUND;
    }
};

/**
 * Generates a room in a random position of random size.
 * @return {Rect}
 */
Map.prototype.generateRoom = function() {
    var width = Math.floor(Math.random() * (MAX_ROOM_SIZE - MIN_ROOM_SIZE) + MIN_ROOM_SIZE),
        height = Math.floor(Math.random() * (MAX_ROOM_SIZE - MIN_ROOM_SIZE) + MIN_ROOM_SIZE),
        positionX = Math.floor(Math.random() * (MAX_WIDTH - width)),
        positionY = Math.floor(Math.random() * (MAX_HEIGHT - height));
    console.log('Creating room of size: ' + width + 'x' + height + ' at [' + positionX + ',' + positionY + ']');

    var newRoom = new Rect(positionX, positionY, width, height);

    return newRoom;
};

Map.prototype.print = function() {
    console.log('Printing map');
    for (var x = 0; x < MAX_WIDTH; x++) {
        for (var y = 0; y < MAX_HEIGHT; y++) {
            if (this.terrain[x][y].type === WALL) {
                process.stdout.write((this.terrain[x][y].type + ' ').bgBlack);
            } else {
                process.stdout.write((this.terrain[x][y].type + ' ').bgWhite.black);
            }
        }
        process.stdout.write('\n');
    }

    // For testing, write terrain array as json to text file
    var valArray = _.map(this.terrain, function(row) {
        return _.map(row, function(tile) {
            return tile.type;
        });
    });

    fs.writeFile('output.txt', JSON.stringify(valArray), function() {
        console.log('file saved');
    });
};

module.exports = Map;