pragma solidity ^0.8.0;

import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/cryptography/MerkleProof.sol";
import "hardhat/console.sol";

contract Voting is Ownable {
    bytes32 private merkleRoot;

    struct Ballot {
        string data;
        uint128 yesCount;
        uint128 noCount;
    }

    uint256[] private ballotIds;
    bytes32 private value;

    mapping(uint256 => Ballot) public IdToBallot; 
    mapping(address => mapping(uint256 => bool)) public UserVoice; //user => ballot id => flag (aready voted or no)

    function toBytes32(address user) view internal returns (bytes32) {
        console.log(user);
        return keccak256(abi.encodePacked(user));
    }

    function setMerkleRoot(bytes32 newRoot) external onlyOwner {
        console.log(123);
        merkleRoot = newRoot;
    }

    //modifire check that user are in whitelist using merkle tree 
    modifier inWhitelist(bytes32[] calldata merkleProof) {
        console.log(456);
        require(MerkleProof.verify(merkleProof, merkleRoot, toBytes32(msg.sender)), "invalid merkle proof");
        _;
    }

    function createVoting(string calldata _data, bytes32[] calldata merkleProof) external inWhitelist(merkleProof) returns (uint256) {
        uint256 id = uint256(keccak256(abi.encode(msg.sender)));
        console.log(id);
        IdToBallot[id] = Ballot(_data, 0, 0);
        ballotIds.push(id);
        return id;
    }

    function voteFor(uint256 ballotId, bytes32[] calldata merkleProof) external inWhitelist(merkleProof) {
        require(UserVoice[msg.sender][ballotId] != true, "this user already voted");
        IdToBallot[ballotId].yesCount++;
        UserVoice[msg.sender][ballotId] = true;
        checkYesCount(ballotId);
    }

    function voteAgainst(uint256 ballotId, bytes32[] calldata merkleProof) external inWhitelist(merkleProof) {
        require(UserVoice[msg.sender][ballotId] != true, "this user already voted");
        IdToBallot[ballotId].noCount++;
        UserVoice[msg.sender][ballotId] = true;
    }

    function checkYesCount(uint256 ballotId) internal {
        if (IdToBallot[ballotId].yesCount >= 5) {
            removeVoting(ballotId);
        }
    }

    function removeVoting(uint256 ballotId) internal {
        delete IdToBallot[ballotId];
    }

    function getIdsArray() external view returns (uint256[] memory) {
           return ballotIds;
    }
}