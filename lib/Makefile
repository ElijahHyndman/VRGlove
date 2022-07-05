all: Comm

# Compiling C# Libraries
# C# lib files located in folder named {NAME}Lib
# Creates .dll
%Lib : FORCE
	mcs -t:library $@/*.cs

# Running Main Files
# See:
# https://www.gnu.org/software/make/manual/make.html#Text-Functions
# For string substitution notes.

# for AppleLib:
#		-invoked using "make runApple"
# 	-Apple.dll located within folder "AppleLib"
# 	-AppleLibMain.cs located within folder "testing"

# Variables
run% : NAME = $(subst run,,$@)# "Comm", for CommLib
run% : LIBRARY_FILE = $(NAME)Lib/$(NAME).dll# dll file location
run% : MAIN_FILE = testing/$(NAME)LibMain# location without .cs or .exe extension
run% :
	@# Compile {NAME}LibMain.cs file inside testing folder using {Name}Lib folder's .dll
	mcs -r:$(LIBRARY_FILE) $(MAIN_FILE).cs
	@# Run {NAME}LibMain.exe
	mono $(MAIN_FILE).exe

# FORCE target
# allows other targets to be built without prerequisites
FORCE:
