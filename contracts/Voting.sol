pragma solidity ^0.8.12;

import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/cryptography/MerkleProof.sol";

contract Voting is Ownable {
    bytes32 private merkleRoot;

    struct Ballot {
        string data;
        uint128 yesCount;
        uint128 noCount;
    }

    mapping(uint256 => Ballot) public IdToBallot; 
    mapping(address => mapping(uint256 => bool)) public UserVoice; //user => ballot id => flag (aready voted or no)

    function toBytes32(address user) view internal onlyOwner returns (bytes32) {
        return bytes32(uint256(uint160(user)));
    }

    //modifire check that user are in whitelist using merkle tree 
    modifier inWhitelist(bytes32[] calldata merkleProof) {
        require(MerkleProof.verify(merkleProof, merkleRoot, toBytes32(msg.sender)) == true, "invalid merkle proof");
        _;
    }

    function createVoiting(string calldata _data, bytes32[] calldata merkleProof) external inWhitelist(merkleProof) {
        uint256 id = uint256(keccak256(abi.encode(_data)));
        IdToBallot[id] = Ballot(_data, 0, 0);
    }

    function voteFor(uint256 ballotId, bytes32[] calldata merkleProof) external inWhitelist(merkleProof) {
        IdToBallot[ballotId].yesCount++;
        UserVoice[msg.sender][ballotId] = true;
        checkYesCount(ballotId);
    }

    function voteAgainst(uint256 ballotId, bytes32[] calldata merkleProof) external inWhitelist(merkleProof) {
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
}