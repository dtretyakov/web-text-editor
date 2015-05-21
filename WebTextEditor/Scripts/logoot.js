(function(EventEmitter, window) {

    var MAX = 32;
    var FIRST = [0];
    var LAST = [undefined];

    // maximum depth of ids
    var maxDepth = 1;

    // counter incremented on each insert operation
    var clock = 0;

    // list of number generators on the specified range
    var numGenerators = [randomIntBtwn, boundaryHighIntBtwn, boundaryLowerIntBtwn];

    /**
     * @param @optional {Object} atoms
     */
    function Logoot(atoms) {
        EventEmitter.apply(this);

        // this.atoms contains the contents that compose your linear data. this.atoms
        // maps an atom id to an atom. An atom can be any data type.
        this.atoms = atoms || {};

        var ids = getIdentifiers(Object.keys(this.atoms));

        // determine maximum id depth
        maxDepth = getIdentifiersDepth(ids);

        // wrap ids in the tokens
        ids.unshift(FIRST);
        ids.push(LAST);

        // this.ids is an ordered index of atom ids. If has pseudo-atoms FIRST and
        // LAST to denote the begining and ending boundaries of our linear data.
        this.ids = ids;
    }

    Logoot.prototype = Object.create(EventEmitter.prototype);

    /**
     * Inserts an `atom` with a given `id`, with the insertion being annotated as
     * being authored by `agent`
     *
     * @param {[Number, String]} id
     * @param {Object} atom
     * @param {String} agent
     * @param @optional {Number} insertAfter
     */
    Logoot.prototype.ins = function(id, atom, agent, insertAfter) {
        var ids = this.ids;
        if (typeof insertAfter == "undefined") {
            insertAfter = indexOfGreatestLessThan(ids, id, compare);
        }
        ids.splice(insertAfter + 1, 0, id);
        this.atoms[hashId(id)] = atom;
        if (arguments.length < 4) {
            this.emit("ins", insertAfter, atom);
        }
        return ["ins", id, atom, agent];
    };

    /**
     * Deletes an atom with the given `id`, with the deletion being annotated as
     * being authored by `agent`. For efficiency's sake, we can optionally pass in
     * `indexOfId`, if it was calculated before the invocation, which allows us to
     * avoid looking up the location of id in our index, this.ids.
     *
     * @param {[Number, String]} id is the id of the atom we want to delete
     * @param {String} agent is the author of the deletion
     * @param @optional {Number} indexOfId is the index of id in this.ids
     */
    Logoot.prototype.del = function(id, agent, indexOfId) {
        var ids = this.ids;
        indexOfId || (indexOfId = indexOf(ids, id, compare));
        ids.splice(indexOfId, 1);
        delete this.atoms[hashId(id)];
        if (arguments.length < 3) {
            this.emit("del", indexOfId - 1);
        }
        return ["del", id, agent];
    };

    /**
     * Applies an operation that is typically received from another author.
     *
     * @param {Array} op can be either ['ins', id, line, agent] or ['del', id, agent]
     */
    Logoot.prototype.applyOp = function(op) {
        var args = op.slice(1);
        if (typeof args[0] == "string") {
            args[0] = deserializeId(args[0]);
        }
        this[op[0]].apply(this, args);
    };

    /**
     * Reduces over the sequence of atoms encapsulated by this Logoot. This is
     * handy when we want to convert our Logoot into a data structure more suitable
     * for our app (e.g., to String of character atoms or to an Array of atoms).
     *
     * @param {Function} fn(reducedValue, atom)
     * @param {init} is the initial value of reducedValue for fn
     * @return {Object} the reduced value
     */
    Logoot.prototype.reduce = function(fn, init) {
        var atoms = this.atoms;
        return this.ids.reduce(function(val, id) {
            if (id === FIRST || id === LAST) return val;
            return fn(val, atoms[hashId(id)]);
        }, init);
    };

    /**
     * Generates an id that is between from and to.
     *
     * @param {Array} from is an atom id currently stored in this.atoms
     * @param {Array} to is an atom id currently stored in this.atoms
     * @param {String} agent is the id of the author creating the id
     * @return {Array} id between from and to
     */
    Logoot.prototype.genId = function(from, to, agent, depth) {

        depth = depth || 1;

        // base depends on depth of identifiers
        maxDepth = Math.max(maxDepth, depth);
        var base = MAX * Math.pow(2, maxDepth);

        var min = from[0] || 0;
        var max = to[0] || base;

        if (min < base - 1) {
            var id = generateIntBtwn(min, max);
            return [id, agent, clock++];
        }

        return [from[0] || 0, from[1] || agent, from[2] || 0]
            .concat(this.genId(from.slice(3), to.slice(3), agent, depth + 1));
    };

    /**
     * Generates a new integer between min and max, non-inclusive.
     *
     * @param {Number} min
     * @param {Number} max
     * @return {Number} random number
     */
    function generateIntBtwn(min, max) {
        var generatorId = Math.floor(Math.random() * numGenerators.length);
        var generator = numGenerators[generatorId];

        return generator(min, max);
    }

    /**
     * Generates a random integer between min and max, non-inclusive.
     *
     * @param {Number} min
     * @param {Number} max
     * @return {Number} random number
     */
    function randomIntBtwn(min, max) {
        return Math.floor(Math.random() * (max - min - 1)) + min + 1;
    }

    /**
     * Generates a random integer between min and max, non-inclusive
     * with a lower boundary constraint.
     *
     * @param {Number} min
     * @param {Number} max
     * @return {Number} random number
     */
    function boundaryLowerIntBtwn(min, max) {
        return Math.floor(Math.random() * (max - min - 1)) + min + 1;
    }

    /**
     * Generates a random integer between min and max, non-inclusive
     * with a high boundary constraint.
     *
     * @param {Number} min
     * @param {Number} max
     * @return {Number} random number
     */
    function boundaryHighIntBtwn(min, max) {
        return Math.floor(Math.random() * (max - min - 1)) + min + 1;
    }

    /**
     * Returns the index of `x` inside of the array `xs`. Returns -1 if not found.
     *
     * @param {Array} xs is an array of objects
     * @param {Object} x is the member of xs that we are trying to locate
     * @param {Function} compare(a, b) returns -1 if a < b; 1 if a > b; 0 if a == b
     * @return {Number} index of x in xs. -1 if x is not in xs.
     */
    function indexOf(xs, x, comparer) {
        for (var i = 0, l = xs.length; i < l; i++) {
            if (comparer(xs[i], x) === 0) return i;
        }
        return -1;
    }

    /**
     * Returns the index of the largest element in `xs`, that is less than `x`,
     * according to a comparison function `compare`. Whether of not `x` is in `xs`,
     * the result will be the same, regardless.
     *
     * @param {Array} xs is an array of objects
     * @param {Object} x is the object to which members of `xs` are being compared
     * @param {Function} compare(a, b) returns -1 if a < b; 1 if a > b; 0 if a == b
     * @return {Number}
     */
    function indexOfGreatestLessThan(xs, x, comparer) {
        var comp, i, l;
        for (i = 0, l = xs.length; i < l; i++) {
            comp = comparer(xs[i], x);
            if (comp !== -1) {
                return i - 1;
            }
        }
        return i;
    }

    /**
     * Compares 2 ids of the form [Number, String]. `a` is less than `b`
     * if a[0] < b[0]. If a[0] == b[0], then `a` is less than `b` iff a[1] < b[1].
     * Returns -1 if a < b; returns 1 if a > b; returns 0 if a == b.
     *
     * @param {Array} a
     * @param {Array} b
     * @return {Number} -1 if a < b; 1 if a > b; 0 if a == b
     */
    function compare(a, b) {
        var depth = Math.max(a.length, b.length);
        var ai, bi;

        for (var i = 0; i < depth; i ++) {
            ai = a[i];
            bi = b[i];

            if (typeof ai == "undefined") return -1;
            if (typeof bi == "undefined") return 1;
            if (ai < bi) return -1;
            if (ai > bi) return 1;
        }

        return 0;
    }

    /**
     * Hashes `id`. Useful for hashing atom ids to store in a hash map.
     *
     * @param {Array} id
     * @return {String}
     */
    function hashId(id) {
        return id.join(".");
    }

    /**
     * Returns a list of composite identifiers from atoms.
     *
     * @param {Object} atoms
     * @return {Array}
     */
    function getIdentifiers(keys) {
        return keys.map(function(key) {
            return deserializeId(key);
        }).sort(compare);
    }

    /**
    * Returns a deserialized id value.
    *
    * @param {String} id - identifier.
    * @return {Array}
    */
    function deserializeId(id) {
        return id.split(".").map(function(val) {
            return parseInt(val);
        });
    }

    /**
    * Returns maximum depth of identfiers.
    *
    * @param {String} id - identifier.
    * @return {Array}
    */
    function getIdentifiersDepth(ids) {
        return ids.reduce(function(max, id) {
            return Math.max(max, id.length / 3);
        }, 1);
    }

    /**
     * Exports logoot.
     */
    window["Logoot"] = Logoot;

})(EventEmitter, window);