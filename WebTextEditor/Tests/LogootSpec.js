describe("Logoout", function() {

    it("should be able to create instance with empty atoms sequence", function() {
        var logoot = new Logoot();
        var str = logoot.reduce(function (str, chr) {
            return str + chr;
        }, "");

        // should contains last and first boundary elements
        expect(Object.keys(logoot.atoms).length).toEqual(0);
        expect(logoot.ids.length).toEqual(2);
        expect(logoot.ids[0]).toEqual([0]);
        expect(logoot.ids[1]).toEqual([64]);

        expect(str).toEqual("");
    });

    it("should be able to create instance with characters sequence", function() {
        var content = { "1.1.1": "a", "2.2.2": "b" };
        var logoot = new Logoot(content);
        var str = logoot.reduce(function(str, chr) {
            return str + chr;
        }, "");

        // should contains last and first boundary elements
        expect(Object.keys(logoot.atoms).length).toEqual(2);
        expect(logoot.atoms["1.1.1"]).toEqual("a");
        expect(logoot.atoms["2.2.2"]).toEqual("b");

        expect(logoot.ids.length).toEqual(4);
        expect(logoot.ids[0]).toEqual([0]);
        expect(logoot.ids[1]).toEqual([1, 1, 1]);
        expect(logoot.ids[2]).toEqual([2, 2, 2]);
        expect(logoot.ids[3]).toEqual([64]);

        expect(str).toEqual("ab");
    });

    describe("when operations executed", function() {
        var logoot;

        beforeEach(function() {
            logoot = new Logoot();
        });

        it("should be able to generate identifiers without boundaries", function() {
            var agent = 777;
            var id = logoot.genId([], [], agent);

            expect(id).not.toBeUndefined();

            expect(id[0] > 0 && id[0] < 64).toBeTruthy();
            expect(id[1]).toEqual(agent);
            expect(id[2]).toEqual(0);
        });

        it("should be able to generate identifiers with boundaries", function() {
            var agent = 777;

            var id = logoot.genId(logoot.ids[0], logoot.ids[1], agent);

            expect(id).not.toBeUndefined();

            expect(id[0] > 0 && id[0] < 64).toBeTruthy();
            expect(id[1]).toEqual(agent);
            expect(id[2]).toEqual(0);
        });

        it("should be able to generate identifiers by increasing it's depth", function () {
            var agent = 777;

            var id = logoot.genId([63, agent, 0], logoot.ids[1], agent);

            expect(id).not.toBeUndefined();

            expect(id[0]).toEqual(63);
            expect(id[1]).toEqual(agent);
            expect(id[2]).toEqual(0);
            expect(id[3] > 0 && id[0] < 64).toBeTruthy();
            expect(id[4]).toEqual(agent);
            expect(id[5]).toEqual(0);
        });

        it("should be able to insert atom", function() {
            var agent = 777;
            var chr = "A";

            var id = logoot.genId([], [], agent);
            var op = logoot.ins(id, chr, agent);
            var str = logoot.reduce(function (str, chr) {
                return str + chr;
            }, "");

            expect(op).not.toBeUndefined();
            expect(op.index).toEqual(0);
            expect(op.id).toEqual(id.join("."));
            expect(op.atom).toEqual(chr);

            expect(str).toEqual(chr);
        });

        it("should be able to delete atom", function () {
            var agent = 777;
            var chr = "A";
            var id = logoot.genId([], [], agent);
            logoot.ins(id, chr, agent);

            var op = logoot.del(id, agent);
            var str = logoot.reduce(function (str, chr) {
                return str + chr;
            }, "");

            expect(op).not.toBeUndefined();
            expect(op.index).toEqual(0);
            expect(op.id).toEqual(id.join("."));

            expect(str).toEqual("");
        });

        it("should be able to apply ops", function () {
            var operation = ["ins", "1.1.1", "a"];

            var op = logoot.applyOp(operation);
            var str = logoot.reduce(function (str, chr) {
                return str + chr;
            }, "");

            expect(op).not.toBeUndefined();
            expect(op.index).toEqual(0);
            expect(op.id).toEqual(operation[1]);

            expect(str).toEqual(operation[2]);
        });
    });
});