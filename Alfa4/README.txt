Nastav konfiguraci programu v App.conifg , který se nachází v \Alfa4\bin\Debug\net6.0
Zde je výchozí nastavení , není potřeba měnit:

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<peerSettings>
		<peerId>PlantationOwner</peerId>
		<discoveryPort>9876</discoveryPort>
		<discoveryIntervalSeconds>5</discoveryIntervalSeconds>
	</peerSettings>
</configuration>

Spuštění:

1.Připoj se na VM. Pokud se připojuješ na vlastní VM nejdříve pokračuj podle návodu na dependencies.Další možností je pokračovat dále a připojit se na: 
ssh -p 20439 jouda@dev.spsejecna.net  # s-prochazka6-3
heslo: jooouda
2. Zadávej následující příkazy:
	cd Alfa4
	
	cd Alfa4

	cd bin

	cd Debug

	cd net6.0
	
	export DOTNET_ROOT=$HOME/.dotnet

	export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools

	dotnet Alfa4.dll
3.Program se automaticky spustí. Posílá UDP discovery a čeká na response. Pokud ho nalezne navazuje TCP spojeni.Jelikož je program silně nevyzpytatelný doporučuji zkusit spustit několikrát pro objevení funkčnosti.

Dependencies:

Zadávej tyto příkazy:
1.	sudo apt-get update
2.	sudo apt-get install git-all
3.	wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
4.	chmod +x ./dotnet-install.sh
5.	./dotnet-install.sh --channel 6.0
6.	git clone https://github.com/Crusader5033/Alfa4.git

Můžeš pokračovat podle návodu na spuštení, krokem 2.
