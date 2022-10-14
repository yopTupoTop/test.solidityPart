const { expect } = require("chai").expect;
const { ethers} = require("hardhat");
const { MerkleTree } = require("merkletreejs");
const keccak256 = require("keccak256");

describe("Voting tests", () => {
    let Voting;

    let voting;

    let leafNodes;
    let merkleTree;

    beforeEach(async () => {
        Voting = await ethers.getContractFactory("Voting");
        voting = await Voting.deploy();

        [owner, address1, address2, address3] = await ethers.getSigners();
        const whitelist = [address1.address, address2.address]

        // =============== Creating whitelist ===============
        leafNodes = whitelist.map(addr => keccak256(addr));
        merkleTree = new MerkleTree(leafNodes, keccak256, {sortPairs: true});
        await voting.setMerkleRoot(merkleTree.getRoot());
    });

    it("should create new yes vote for ballot", async () => {
        let ballotId;
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        voting.IdToBallot[ballotId] = await voting.createVoting("voting for any", merkleProof1, {from: address1});
        let hashedAddress2 = keccak256(address2.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        await voting.voteFor(ballotId, merkleProof2, {from: address2});
        expect(voting.IdToBallot[ballotId].yesCount).to.equal(1);
        expect(voting.UserVoise[address2][ballotId]).to.equal(true);
    });

    it("should create new no vote for ballot", async () => {
        let ballotId;
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        voting.IdToBallot[ballotId] = await voting.createVoting("voting for any", merkleProof1, {from: address1});
        let hashedAddress2 = keccak256(address2.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        await voting.voteAgainst(ballotId,merkleProof2, {from: address2});
        expect(voting.IdToBallot[ballotId].noCount).to.equal(1);
        expect(voting.UserVoise[address2][ballotId]).to.equal(true);
    });

    it("try create new voting not from whitelist", async () => {
        let hashedAddress = keccak256(address3.address);
        let merkleProof = merkleTree.getHexProof(hashedAddress);
        await expect(voting.createVoting("voting for any", merkleProof, {from: address3})).revertedWith("invalid merkle proof");
    });

    it("try to create new yes vote not from whitelist", async () => {
        let ballotId;
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        voting.IdToBallot[ballotId] = await voting.createVoting("voting for any", merkleProof1, {from: address1});
        let hashedAddress2 = keccak256(address3.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        await expect(voting.voteFor(ballotId, merkleProof2, {from: address3})).revertedWith("invalid merkle proof");
    });

    it("try to create new no vote not from whitelist", async () => {
        let ballotId;
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        voting.IdToBallot[ballotId] = await voting.createVoting("voting for any", merkleProof1, {from: address1});
        let hashedAddress2 = keccak256(address3.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        await expect(voting.voteAgainst(ballotId, merkleProof2, {from: address3})).revertedWith("invalid merkle proof");
    });
})