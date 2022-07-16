all:
	mcs -t:library -out:VRGlove.dll *.cs

# Compiling C# Libraries
# C# lib files located in folder named {NAME}Lib
# Creates .dll
# %Lib : FORCE
# 	mcs -t:library $@/*.cs

# Running Main Files
# See:
# https://www.gnu.org/software/make/manual/make.html#Text-Functions
# For string substitution notes.

# for AppleLib:
#		-invoked using "make runApple"
# 	-Apple.dll located within folder "AppleLib"
# 	-AppleLibMain.cs located within folder "testing"

# Variables
run% : NAME = $(subst run,,$@)#Remove "run" from name
run% : LIBRARY_FILE = VRGlove.dll# dll file location
run% : MAIN_FILE = testing/$(NAME)LibMain# location without .cs or .exe extension
run% :
	@clear
	@echo "Compiling==================================="
	mcs -t:library -out:$(LIBRARY_FILE) *.cs
	@# Compile {NAME}LibMain.cs file inside testing folder using {Name}Lib folder's .dll
	mcs -r:$(LIBRARY_FILE) $(MAIN_FILE).cs
	@# Move required .dll to execution folder
	cp $(LIBRARY_FILE) testing/$(LIBRARY_FILE)
	@# Run {NAME}LibMain.exe
	mono $(MAIN_FILE).exe

# FORCE target
# allows other targets to be built without prerequisites
FORCE:
