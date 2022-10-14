const { expect } = require("chai");
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
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        const txResponce = await voting.connect(address1).createVoting("voting for any", merkleProof1);
        const txReceipt = await txResponce.wait();
        const [ballotCreatedEvent] = txReceipt.events;
        const {id} = ballotCreatedEvent.args;
        let hashedAddress2 = keccak256(address2.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        console.log(id);
        console.log(merkleProof2);
        await voting.connect(address2).voteFor(id, merkleProof2);
        let {yesCount} = await voting.IdToBallot(id);
        console.log(yesCount);
        expect(yesCount).to.equal(ethers.BigNumber.from(1));
        expect(await voting.UserVoice(address2.address, id)).to.equal(true);
    });

    it("should create new no vote for ballot", async () => {
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        const txResponce = await voting.connect(address1).createVoting("voting for any", merkleProof1);
        const txReceipt = await txResponce.wait();
        const [ballotCreatedEvent] = txReceipt.events;
        const {id} = ballotCreatedEvent.args;
        let hashedAddress2 = keccak256(address2.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        console.log(id);
        console.log(merkleProof2);
        await voting.connect(address2).voteAgainst(id, merkleProof2);
        let {noCount} = await voting.IdToBallot(id);
        console.log(noCount);
        expect(noCount).to.equal(ethers.BigNumber.from(1));
        expect(await voting.UserVoice(address2.address, id)).to.equal(true);
    });

    it("try create new voting not from whitelist", async () => {
        let hashedAddress = keccak256(address3.address);
        let merkleProof = merkleTree.getHexProof(hashedAddress);
        await expect(voting.connect(address3).createVoting("voting for any", merkleProof)).revertedWith("invalid merkle proof");
    });

    it("try to create new yes vote not from whitelist", async () => {
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        const txResponce = await voting.connect(address1).createVoting("voting for any", merkleProof1);
        const txReceipt = await txResponce.wait();
        const [ballotCreatedEvent] = txReceipt.events;
        const {id} = ballotCreatedEvent.args;
        let hashedAddress2 = keccak256(address3.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        await expect(voting.connect(address3).voteFor(id, merkleProof2)).revertedWith("invalid merkle proof");
    });

    it("try to create new no vote not from whitelist", async () => {
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        const txResponce = await voting.connect(address1).createVoting("voting for any", merkleProof1);
        const txReceipt = await txResponce.wait();
        const [ballotCreatedEvent] = txReceipt.events;
        const {id} = ballotCreatedEvent.args;
        let hashedAddress2 = keccak256(address3.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        await expect(voting.connect(address3).voteAgainst(id, merkleProof2)).revertedWith("invalid merkle proof");
    });

    it("try to create new vote than user already voted", async () => {
        let hashedAddress1 = keccak256(address1.address);
        let merkleProof1 = merkleTree.getHexProof(hashedAddress1);
        const txResponce = await voting.connect(address1).createVoting("voting for any", merkleProof1);
        const txReceipt = await txResponce.wait();
        const [ballotCreatedEvent] = txReceipt.events;
        const {id} = ballotCreatedEvent.args;
        let hashedAddress2 = keccak256(address2.address);
        let merkleProof2 = merkleTree.getHexProof(hashedAddress2);
        console.log(id);
        console.log(merkleProof2);
        await voting.connect(address2).voteAgainst(id, merkleProof2);
        await expect(voting.connect(address2).voteFor(id, merkleProof2)).revertedWith("this user already voted");
        await expect(voting.connect(address2).voteAgainst(id, merkleProof2)).revertedWith("this user already voted");
    })
})