pragma solidity ^0.8.12;

contract Voting {

    struct Ballot {
        string data;
        uint128 yesCount;
        uint128 noCount;
    }

    mapping(uint256 => Ballot) public IdToBallot; 
    mapping(address => mapping(uint256 => bool)) public UserVoice; //user => ballot id => flag (aready voted or no)

    function createVoiting(string calldata _data) external {
        uint256 id = uint256(keccak256(abi.encode(_data)));
        IdToBallot[id] = Ballot(_data, 0, 0);
    }

    function voteFor(uint256 ballotId) external {
        IdToBallot[ballotId].yesCount++;
        UserVoice[msg.sender][ballotId] = true;
        checkYesCount(ballotId);
    }

    function voteAgainst(uint256 ballotId) external {
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