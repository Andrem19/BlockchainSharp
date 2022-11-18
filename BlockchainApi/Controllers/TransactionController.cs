using BlockchainSharp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BlockchainApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        public Node _node { get; set; }
        public TransactionController(Node node)
        {
            _node = node;
        }
        [HttpGet("getblockchain")]
        public ActionResult<Blockchain> GetBlockchain()
        {
            return Ok(_node._blockchain);
        }
        [HttpPost]
        public async Task<ActionResult> Transaction([FromBody]Transaction transaction)
        {
            bool res = _node.HandleTransaction(transaction);
            if (!res) BadRequest("Bed request");
            return Ok();
        }
        [HttpGet("gettransactions")]
        public ActionResult<TransactionPool> GetTransactions()
        {
            return Ok(_node._transactionPool);
        }
        [HttpGet("send")]
        public async Task<ActionResult> SendTransaction()
        {
            Wallet bob = new Wallet();
            Wallet alice = new Wallet();
            Wallet exchange = new Wallet();

            Transaction transaction = exchange.CreateTransaction(alice.GetPublicKeyString(), 10, "EXCHANGE");
            string trToSend = transaction.toJson();
            HttpClient client = new HttpClient();
            var bodyContent = new StringContent(trToSend, Encoding.UTF8, "application/json");
            await client.PostAsync("http://localhost:10002/Transaction", bodyContent);
            return Ok(bodyContent);
        }
    }
}
