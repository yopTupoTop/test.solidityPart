const { expect } = require("chai").expect;
const { ethers} = require("hardhat");

describe("Voting tests", () => {
    let Voting;

    let voting;

    beforeEach(async () => {
        Voting = await ethers.getContractFactory("Voting");
        voting = await Voting.deploy();

        [owner, address1, address2] = await ethers.getSigners();
    })

    it("should create new yes vote for ballot", async () => {
        let ballotId;
        let voice;
        voting.IdToBallot[ballotId] = await voting.createVoting("voting for any", {from: address1})
        voice = await voting.voteFor(ballotId, {from: address2});
        expect(voting.IdToBallot[ballotId].yesCount).to.equal(1);
        expect(voting.UserVoise[address2][ballotId]).to.equal(true);
    })

    it("should create new no vote for ballot", async () => {
        let ballotId;
        let voice;
        voting.IdToBallot[ballotId] = await voting.createVoting("voting for any", {from: address1})
        voice = await voting.voteAgainst(ballotId, {from: address2});
        expect(voting.IdToBallot[ballotId].noCount).to.equal(1);
        expect(voting.UserVoise[address2][ballotId]).to.equal(true);
    })
})