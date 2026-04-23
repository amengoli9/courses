/*
 * ============================================================================
 * 7. CONNASCENCE OF TIMING (CoTi)
 * ============================================================================
 *
 * DEFINIZIONE: Il TIMING dell'esecuzione di più componenti è importante
 * FORZA: ⭐⭐⭐⭐⭐ MOLTO ALTA (estremamente pericolosa!)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    Thread 1: balance = balance + 100;
    Thread 2: balance = balance - 50;
    // Race condition! Risultato dipende dal timing

SOLUZIONE:
    lock (balanceLock) {
        balance = balance + 100;
    }
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfTiming
{
    // ❌ PROBLEMA: Race condition su stato condiviso
    public class BankAccount
    {
        private decimal _balance = 1000m;

        public void Deposit(decimal amount)
        {
            // Non thread-safe!
            var temp = _balance;
            Thread.Sleep(1); // Simula elaborazione
            _balance = temp + amount;
            Console.WriteLine($"Deposited {amount}, balance: {_balance}");
        }

        public void Withdraw(decimal amount)
        {
            // Non thread-safe!
            var temp = _balance;
            Thread.Sleep(1); // Simula elaborazione
            _balance = temp - amount;
            Console.WriteLine($"Withdrawn {amount}, balance: {_balance}");
        }

        public decimal GetBalance() => _balance;
    }

    public class Example
    {
        public void Test()
        {
            var account = new BankAccount();

            // Due thread che accedono allo stesso account
            var t1 = new Thread(() => account.Deposit(100));
            var t2 = new Thread(() => account.Withdraw(50));

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            // Risultato imprevedibile! Potrebbe essere 1050, 1100, 950...
            Console.WriteLine($"Final balance: {account.GetBalance()}");
        }
    }

    // ✅ SOLUZIONE 1: Lock per sincronizzazione
    public class BankAccountRefactored1
    {
        private decimal _balance = 1000m;
        private readonly object _lock = new object();

        public void Deposit(decimal amount)
        {
            lock (_lock) // Garantisce accesso esclusivo
            {
                var temp = _balance;
                Thread.Sleep(1);
                _balance = temp + amount;
                Console.WriteLine($"Deposited {amount}, balance: {_balance}");
            }
        }

        public void Withdraw(decimal amount)
        {
            lock (_lock)
            {
                var temp = _balance;
                Thread.Sleep(1);
                _balance = temp - amount;
                Console.WriteLine($"Withdrawn {amount}, balance: {_balance}");
            }
        }

        public decimal GetBalance()
        {
            lock (_lock)
            {
                return _balance;
            }
        }
    }

    // ✅ SOLUZIONE 2: Immutabilità (nessuno stato mutabile condiviso)
    public record BankAccountState(decimal Balance);

    public class BankAccountRefactored2
    {
        private BankAccountState _state = new BankAccountState(1000m);
        private readonly object _lock = new object();

        public BankAccountState Deposit(decimal amount)
        {
            lock (_lock)
            {
                // Crea nuovo stato invece di modificare
                _state = new BankAccountState(_state.Balance + amount);
                Console.WriteLine($"Deposited {amount}, balance: {_state.Balance}");
                return _state;
            }
        }

        public BankAccountState Withdraw(decimal amount)
        {
            lock (_lock)
            {
                _state = new BankAccountState(_state.Balance - amount);
                Console.WriteLine($"Withdrawn {amount}, balance: {_state.Balance}");
                return _state;
            }
        }

        public BankAccountState GetState() => _state;
    }

    // ✅ SOLUZIONE 3: Actor Model / Message Queue
    public class BankAccountMessage
    {
        public enum MessageType { Deposit, Withdraw, GetBalance }
        public MessageType Type { get; set; }
        public decimal Amount { get; set; }
        public TaskCompletionSource<decimal> Response { get; set; }
    }

    public class BankAccountActor
    {
        private decimal _balance = 1000m;
        private readonly BlockingCollection<BankAccountMessage> _messageQueue = new();

        public BankAccountActor()
        {
            // Thread dedicato per processare messaggi (uno alla volta!)
            Task.Run(() => ProcessMessages());
        }

        private void ProcessMessages()
        {
            foreach (var msg in _messageQueue.GetConsumingEnumerable())
            {
                switch (msg.Type)
                {
                    case BankAccountMessage.MessageType.Deposit:
                        _balance += msg.Amount;
                        Console.WriteLine($"Deposited {msg.Amount}, balance: {_balance}");
                        msg.Response?.SetResult(_balance);
                        break;

                    case BankAccountMessage.MessageType.Withdraw:
                        _balance -= msg.Amount;
                        Console.WriteLine($"Withdrawn {msg.Amount}, balance: {_balance}");
                        msg.Response?.SetResult(_balance);
                        break;

                    case BankAccountMessage.MessageType.GetBalance:
                        msg.Response?.SetResult(_balance);
                        break;
                }
            }
        }

        public Task<decimal> Deposit(decimal amount)
        {
            var tcs = new TaskCompletionSource<decimal>();
            _messageQueue.Add(new BankAccountMessage
            {
                Type = BankAccountMessage.MessageType.Deposit,
                Amount = amount,
                Response = tcs
            });
            return tcs.Task;
        }

        public Task<decimal> Withdraw(decimal amount)
        {
            var tcs = new TaskCompletionSource<decimal>();
            _messageQueue.Add(new BankAccountMessage
            {
                Type = BankAccountMessage.MessageType.Withdraw,
                Amount = amount,
                Response = tcs
            });
            return tcs.Task;
        }
    }

    // VANTAGGI:
    // - Lock: garantisce accesso esclusivo
    // - Immutabilità: nessuna race condition possibile
    // - Actor Model: serializza operazioni automaticamente
}
