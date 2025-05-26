// loanCalculator.js
(function(window, document) {
  const interestRate = 0.40;

  function calculate() {
    const amountInput = document.getElementById('amount');
    const periodInput = document.getElementById('period');
    const monthlyLabel = document.getElementById('monthlyPayment');
    const totalLabel   = document.getElementById('totalPayment');

    const principal = parseFloat(amountInput.value) || 0;
    const months    = parseInt(periodInput.value) || 0;

    if (principal < 300 || principal > 5000 || months < 5 || months > 24) {
      monthlyLabel.textContent = '0.00 лв.';
      totalLabel.textContent   = '0.00 лв.';
      return;
    }

    const monthlyRate = interestRate / 12;
    const monthly     = (principal * monthlyRate) / (1 - Math.pow(1 + monthlyRate, -months));
    const total       = monthly * months;

    monthlyLabel.textContent = monthly.toFixed(2) + ' лв.';
    totalLabel.textContent   = total.toFixed(2) + ' лв.';
  }

  // Експортираме функцията в глобален обект
  window.loanCalculator = {
    calculate,
    init: function(amountSelector, periodSelector) {
      const amountEl = document.querySelector(amountSelector);
      const periodEl = document.querySelector(periodSelector);

      amountEl.addEventListener('input', calculate);
      periodEl.addEventListener('input', calculate);
    }
  };
})(window, document);
