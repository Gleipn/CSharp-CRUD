function consultarCEP() {
    var cep = document.getElementById('Cep').value.replace(/\D/g, '');
    if (cep.length === 8) {
        fetch(`https://viacep.com.br/ws/${cep}/json/`)
            .then(response => response.json())
            .then(data => {
                if (data.erro) {
                    document.getElementById('Endereco').value = 'CEP não encontrado.';
                } else {
                    var enderecoCompleto = `${data.logradouro}, ${data.bairro} - ${data.localidade} / ${data.uf}`;
                    document.getElementById('Endereco').value = enderecoCompleto;
                }
            })
            .catch(error => {
                console.error('Erro ao consultar CEP:', error);
                document.getElementById('Endereco').value = 'Erro ao consultar o CEP.';
            });
    } else {
        document.getElementById('Endereco').value = 'Digite um CEP válido.';
    }
}

function previewImagem(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('outputImagem');
        output.src = reader.result;
    }
    reader.readAsDataURL(event.target.files[0]);
}

function validateCNPJ(input) {
    
    input.value = input.value.replace(/[^\d]/g, '');

    
    if (input.value.length > 14) {
        input.value = input.value.slice(0, 14);
    }
}