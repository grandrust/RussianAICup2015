sudo apt-get update
sudo apt-get install unzip
sudo apt-get -qqy install default-jdk
   
cd /vagrant
mkdir doc
cd doc/
wget http://russianaicup.ru/s/1447999864318/assets/documentation/coderacing2015-docs.pdf
cd ../
mkdir Runner
cd Runner/
wget http://russianaicup.ru/s/1448458279522/assets/local-runner/local-runner.zip
unzip local-runner.zip
sudo rm -f local-runner.zip
cd ../
mkdir Repeater
cd Repeater/
wget http://russianaicup.ru/s/1448458279522/assets/repeater/repeater.zip
unzip repeater.zip
sudo rm -f repeater.zip