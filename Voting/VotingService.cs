using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using SolidityPart.Contracts.Voting.ContractDefinition;

namespace SolidityPart.Contracts.Voting
{
    public partial class VotingService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, VotingDeployment votingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<VotingDeployment>().SendRequestAndWaitForReceiptAsync(votingDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, VotingDeployment votingDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<VotingDeployment>().SendRequestAsync(votingDeployment);
        }

        public static async Task<VotingService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, VotingDeployment votingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, votingDeployment, cancellationTokenSource);
            return new VotingService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public VotingService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<IdToBallotOutputDTO> IdToBallotQueryAsync(IdToBallotFunction idToBallotFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<IdToBallotFunction, IdToBallotOutputDTO>(idToBallotFunction, blockParameter);
        }

        public Task<IdToBallotOutputDTO> IdToBallotQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var idToBallotFunction = new IdToBallotFunction();
                idToBallotFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<IdToBallotFunction, IdToBallotOutputDTO>(idToBallotFunction, blockParameter);
        }

        public Task<bool> UserVoiceQueryAsync(UserVoiceFunction userVoiceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<UserVoiceFunction, bool>(userVoiceFunction, blockParameter);
        }

        
        public Task<bool> UserVoiceQueryAsync(string returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var userVoiceFunction = new UserVoiceFunction();
                userVoiceFunction.ReturnValue1 = returnValue1;
                userVoiceFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<UserVoiceFunction, bool>(userVoiceFunction, blockParameter);
        }

        public Task<string> CreateVotingRequestAsync(CreateVotingFunction createVotingFunction)
        {
             return ContractHandler.SendRequestAsync(createVotingFunction);
        }

        public Task<TransactionReceipt> CreateVotingRequestAndWaitForReceiptAsync(CreateVotingFunction createVotingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createVotingFunction, cancellationToken);
        }

        public Task<string> CreateVotingRequestAsync(string data, List<byte[]> merkleProof)
        {
            var createVotingFunction = new CreateVotingFunction();
                createVotingFunction.Data = data;
                createVotingFunction.MerkleProof = merkleProof;
            
             return ContractHandler.SendRequestAsync(createVotingFunction);
        }

        public Task<TransactionReceipt> CreateVotingRequestAndWaitForReceiptAsync(string data, List<byte[]> merkleProof, CancellationTokenSource cancellationToken = null)
        {
            var createVotingFunction = new CreateVotingFunction();
                createVotingFunction.Data = data;
                createVotingFunction.MerkleProof = merkleProof;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createVotingFunction, cancellationToken);
        }

        public Task<List<BigInteger>> GetIdsArrayQueryAsync(GetIdsArrayFunction getIdsArrayFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetIdsArrayFunction, List<BigInteger>>(getIdsArrayFunction, blockParameter);
        }

        
        public Task<List<BigInteger>> GetIdsArrayQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetIdsArrayFunction, List<BigInteger>>(null, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<string> RenounceOwnershipRequestAsync(RenounceOwnershipFunction renounceOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        }

        public Task<string> RenounceOwnershipRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceOwnershipFunction>();
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public Task<string> SetMerkleRootRequestAsync(SetMerkleRootFunction setMerkleRootFunction)
        {
             return ContractHandler.SendRequestAsync(setMerkleRootFunction);
        }

        public Task<TransactionReceipt> SetMerkleRootRequestAndWaitForReceiptAsync(SetMerkleRootFunction setMerkleRootFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMerkleRootFunction, cancellationToken);
        }

        public Task<string> SetMerkleRootRequestAsync(byte[] newRoot)
        {
            var setMerkleRootFunction = new SetMerkleRootFunction();
                setMerkleRootFunction.NewRoot = newRoot;
            
             return ContractHandler.SendRequestAsync(setMerkleRootFunction);
        }

        public Task<TransactionReceipt> SetMerkleRootRequestAndWaitForReceiptAsync(byte[] newRoot, CancellationTokenSource cancellationToken = null)
        {
            var setMerkleRootFunction = new SetMerkleRootFunction();
                setMerkleRootFunction.NewRoot = newRoot;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMerkleRootFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> VoteAgainstRequestAsync(VoteAgainstFunction voteAgainstFunction)
        {
             return ContractHandler.SendRequestAsync(voteAgainstFunction);
        }

        public Task<TransactionReceipt> VoteAgainstRequestAndWaitForReceiptAsync(VoteAgainstFunction voteAgainstFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(voteAgainstFunction, cancellationToken);
        }

        public Task<string> VoteAgainstRequestAsync(BigInteger ballotId, List<byte[]> merkleProof)
        {
            var voteAgainstFunction = new VoteAgainstFunction();
                voteAgainstFunction.BallotId = ballotId;
                voteAgainstFunction.MerkleProof = merkleProof;
            
             return ContractHandler.SendRequestAsync(voteAgainstFunction);
        }

        public Task<TransactionReceipt> VoteAgainstRequestAndWaitForReceiptAsync(BigInteger ballotId, List<byte[]> merkleProof, CancellationTokenSource cancellationToken = null)
        {
            var voteAgainstFunction = new VoteAgainstFunction();
                voteAgainstFunction.BallotId = ballotId;
                voteAgainstFunction.MerkleProof = merkleProof;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(voteAgainstFunction, cancellationToken);
        }

        public Task<string> VoteForRequestAsync(VoteForFunction voteForFunction)
        {
             return ContractHandler.SendRequestAsync(voteForFunction);
        }

        public Task<TransactionReceipt> VoteForRequestAndWaitForReceiptAsync(VoteForFunction voteForFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(voteForFunction, cancellationToken);
        }

        public Task<string> VoteForRequestAsync(BigInteger ballotId, List<byte[]> merkleProof)
        {
            var voteForFunction = new VoteForFunction();
                voteForFunction.BallotId = ballotId;
                voteForFunction.MerkleProof = merkleProof;
            
             return ContractHandler.SendRequestAsync(voteForFunction);
        }

        public Task<TransactionReceipt> VoteForRequestAndWaitForReceiptAsync(BigInteger ballotId, List<byte[]> merkleProof, CancellationTokenSource cancellationToken = null)
        {
            var voteForFunction = new VoteForFunction();
                voteForFunction.BallotId = ballotId;
                voteForFunction.MerkleProof = merkleProof;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(voteForFunction, cancellationToken);
        }
    }
}
