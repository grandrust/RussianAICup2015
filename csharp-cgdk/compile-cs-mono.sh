name="MyStrategy"

if [ ! -f $name.cs ]
then
    echo Unable to find $name.cs > compilation.log
    exit 1
fi

rm -f $name.exe

files=""

for i in *.cs
do
    files="$files $i"
done

for i in Model/*.cs
do
    files="$files $i"
done

for i in Properties/*.cs
do
    files="$files $i"
done

for i in GranDrust/*.cs
do
    files="$files $i"
done

for i in GranDrust/Factories/*.cs
do
    files="$files $i"
done

dmcs -define:ONLINE_JUDGE -o+ -sdk:4 -out:$name.exe -debug $files 2>compilation.log
